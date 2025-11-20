using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.RateCalculation;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.DTOs.RoomStatus;
using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Application.Interfaces;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Application.Services.CurrentUserService;
using HotelApp.Application.Services.RateCalculationService;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using HotelApp.Domain.Enums;
using System.Linq;
using System.ServiceModel.Channels;

namespace HotelApp.Application.Services.ReservationService
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        private readonly IRateCalculationService _rateCalculationService;

        public ReservationService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IRateCalculationService rateCalculationService)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
            _rateCalculationService = rateCalculationService;
        }
		public async Task<IEnumerable<GetAllReservationsDTO>> GetAllReservationAsync()
        {
			var reservation = await _unitOfWork.ReservationRepository
				.GetAllAsDtoAsync<GetAllReservationsDTO>();

			return reservation;
		}
		public async Task<GetReservationDetailsByIdDTO> GetReservationDetailsByIdAsync(int Id)
		{
			var reservation = await _unitOfWork.ReservationRepository.GetReservationDetailsByIdAsync(Id);
			return reservation;
		}
		public async Task<IEnumerable<GetAllReservationsDTO>> GetFilteredReservationsAsync(ReservationFilterDTO dto)
		{
			var reservations = await _unitOfWork.ReservationRepository.GetFilteredReservationsAsync(dto);

			return reservations;
		}
        public async Task<ServiceResponse<object>> ChangeReservationDatesAsync(ChangeReservationDatesDTO dto)
		{
			var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(dto.ReservationId);
			if (reservation == null)
			{
				return ServiceResponse<object>.ResponseFailure("Reservation Not Found.");
			}
			try
			{
				if(dto.CheckIn > dto.CheckOut)
				{
                    return ServiceResponse<object>.ResponseFailure("Check In Date must be before Check Out Date.");
                }
				reservation.CheckInDate = dto.CheckIn;
				reservation.CheckOutDate = dto.CheckOut;

				_unitOfWork.ReservationRepository.Update(reservation);
				await _unitOfWork.CommitAsync();

                return ServiceResponse<object>.ResponseSuccess("Reservation Dates Changed Successfully.");
            }
			catch (Exception ex)
			{
                return ServiceResponse<object>.ResponseFailure(ex.Message);
            }
		}
        public async Task<ReservationDTO> GetReservationToEditByIdAsync(int Id)
		{
			var dto = await _unitOfWork.ReservationRepository.GetReservationToEditByIdAsync(Id);

			return dto;
		}
        public async Task<ServiceResponse<ReservationDTO>> SaveReservation(ReservationDTO dto, int BranchId ,int UserId)
        {
            List<int> RoomIDs = dto.ReservationInfoDto.RoomTypeToBookDTOs.SelectMany(r => r.RoomIds).ToList();
            if (dto.ConfirmDto.IsCheckedIn)
            {
                int expectedRoomCount = dto.ReservationInfoDto.RoomTypeToBookDTOs.Sum(rt => rt.NumOfRooms);
                int selectedRoomCount = RoomIDs.Count();

                if (expectedRoomCount != selectedRoomCount)
                {
                    return ServiceResponse<ReservationDTO>.ResponseFailure($"You must select all {expectedRoomCount} rooms before checking in.");
                }
            }
            if (dto.ConfirmDto.IsCheckedOut && !dto.ConfirmDto.IsCheckedIn)
            {
                return ServiceResponse<ReservationDTO>.ResponseFailure("A reservation cannot be checked out before being checked in.");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var rateCalcRequest = new GetRateCalculationDTORequest
                {
                    RateId = dto.ReservationInfoDto.RateId,
                    ReservationId = dto.ReservationInfoDto.ReservationId,
                    CheckIn = dto.ReservationInfoDto.CheckInDate.Value,
                    CheckOut = dto.ReservationInfoDto.CheckOutDate.Value,
                    RoomTypeQuantities = dto.ReservationInfoDto.RoomTypeToBookDTOs
                         .Select(rt => new RoomTypeQuantityDTO
                         {
                             RoomTypeId = rt.RoomTypeId,
                             Quantity = rt.NumOfRooms
                         }).ToList()
                };

                var rateCalcResponse = await _rateCalculationService.GetRateCalculation(rateCalcRequest);

                if (!rateCalcResponse.Success)
                {
                    return ServiceResponse<ReservationDTO>.ResponseFailure("Failed to calculate rate: " + rateCalcResponse.Message);
                }
                decimal totalPrice = rateCalcResponse.Data.TotalPrice;
                decimal totalPayments = rateCalcResponse.Data.TotalPayments;
                decimal balance = rateCalcResponse.Data.Balance;

                var statuses = ExtractStatuses(dto.ConfirmDto);

                Reservation? reservation = new Reservation();

                if (dto.ReservationInfoDto.ReservationId == 0)
                {
                    // ===== Add New Reservation =====
                    reservation = _mapper.Map<Reservation>(dto);
                    reservation.TotalPrice = totalPrice;
                    reservation.TotalPayments = totalPayments;
                    reservation.Balance = balance;
                    reservation.ReservationNumber = await _unitOfWork.ReservationRepository.GenerateReservationNumberAsync(2);
                    if (statuses != null && statuses.Any())
                        reservation.Status = statuses.Last();

                    await _unitOfWork.ReservationRepository.AddNewAsync(reservation);
                    await _unitOfWork.CommitAsync();

                    await AddReservationHistoryRangeAsync(reservation.Id, statuses, UserId);
                }
                else
                {
                    // ===== Edit Existing Reservation =====
                    reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(dto.ReservationInfoDto.ReservationId);
                    if (reservation == null)
                        return ServiceResponse<ReservationDTO>.ResponseFailure("Reservation not found");

                    _mapper.Map(dto, reservation);
                    reservation.TotalPrice = totalPrice;
                    reservation.TotalPayments = totalPayments;
                    reservation.Balance = balance;
                    if (statuses != null && statuses.Any())
                        reservation.Status = statuses.Last();

                    _unitOfWork.ReservationRepository.Update(reservation);
                    await _unitOfWork.CommitAsync();

                    // Remove old room types and rooms if needed
                    var oldRoomTypes = await _unitOfWork.ReservationRoomTypeRepository
                                            .GetAllAsync(r => r.ReservationId == reservation.Id);
                    _unitOfWork.ReservationRoomTypeRepository.DeleteRange(oldRoomTypes);

                    var oldRooms = await _unitOfWork.ReservationRoomRepository
                                        .GetAllAsync(r => r.ReservationId == reservation.Id);
                    _unitOfWork.ReservationRoomRepository.DeleteRange(oldRooms);

                    var oldGuests = await _unitOfWork.GuestReservationRepository
                                        .GetAllAsync(g => g.ReservationId == reservation.Id);
                    _unitOfWork.GuestReservationRepository.DeleteRange(oldGuests);

                    var existingHistories = await _unitOfWork.ReservationHistoryRepository
                        .GetAllAsync(rh => rh.ReservationId == reservation.Id);

                    var existingStatuses = existingHistories.Select(h => h.Status).ToHashSet();

                    var newStatuses = statuses.Where(s => !existingStatuses.Contains(s)).ToList();

                    if (newStatuses.Any())
                    {
                        await AddReservationHistoryRangeAsync(reservation.Id, newStatuses, UserId);
                    }
                }

                // ===== Reservation Room Types =====
                var reservationRoomTypes = dto.ReservationInfoDto.RoomTypeToBookDTOs.Select(rt => new ReservationRoomType
                {
                    RoomTypeId = rt.RoomTypeId,
                    ReservationId = reservation.Id,
                    Quantity = rt.NumOfRooms,
                    NumOfAdults = rt.NumOfAdults,
                    NumOfChildren = rt.NumOfChildrens
                }).ToList();
                await _unitOfWork.ReservationRoomTypeRepository.AddRangeAsync(reservationRoomTypes);

                // ===== Reservation Rooms =====
                if (RoomIDs.Any())
                {
                    var reservationRooms = RoomIDs.Select(rr => new ReservationRoom
                    {
                        ReservationId = reservation.Id,
                        RoomId = rr,
                        StartDate = reservation.CheckInDate,
                        EndDate = reservation.CheckOutDate
                    });
                    await _unitOfWork.ReservationRoomRepository.AddRangeAsync(reservationRooms);

                    var rooms = await _unitOfWork.RoomRepository
                        .GetAllAsync(r => RoomIDs.Contains(r.Id));

                    if (dto.ConfirmDto.IsCheckedIn)
                    {
                        var systemSettings = await _unitOfWork.SystemSettingRepository.FirstOrDefaultAsync();
                        foreach (var room in rooms)
                            room.RoomStatusId = systemSettings.CheckInStatusId;
                    }
                    else if (dto.ConfirmDto.IsConfirmed || dto.ConfirmDto.IsPending && !dto.ConfirmDto.IsCheckedIn)
                    {
                        var roomStatus = await _unitOfWork.RoomStatusRepository
                            .FirstOrDefaultAsync(rs => rs.Code == RoomStatusEnum.Reserved, SkipBranchFilter: true);
                        foreach (var room in rooms)
                            room.RoomStatusId = roomStatus.Id;
                    }

                    _unitOfWork.RoomRepository.UpdateRange(rooms);
                }

                // ===== Guest Reservations =====
                var guestReservations = dto.GuestDtos.Select((guest, index) => new GuestReservation
                {
                    GuestId = guest.GuestId,
                    ReservationId = reservation.Id,
                    IsPrimaryGuest = dto.GuestDtos[index].IsPrimary
                }).ToList();
                await _unitOfWork.GuestReservationRepository.AddRangeAsync(guestReservations);

                foreach (var rt in dto.ReservationInfoDto.RoomTypeToBookDTOs)
                {
                    var checkIn = dto.ReservationInfoDto.CheckInDate.Value;
                    var checkOut = dto.ReservationInfoDto.CheckOutDate.Value;

                    var availabilityDto = new RoomTypeAvailabilityRequestDTO
                    {
                        BranchId = BranchId,
                        RoomTypeId = rt.RoomTypeId,
                        CheckInDate = checkIn,
                        CheckOutDate = checkOut,
                        ReservationId = dto.ReservationInfoDto.ReservationId
                    };
                    var check = await _unitOfWork.RoomTypeRepository
                        .GetRoomTypeAvailabilityAsync(availabilityDto);

                    if(rt.NumOfRooms > check)
                    {
                        return ServiceResponse<ReservationDTO>.ResponseFailure(
                            $"Room Type ID { rt.RoomTypeId} no longer has enough rooms. Please review your selection.",
                            additionalData: true);
                    }
                }

                await _unitOfWork.CommitTransactionAsync();

                return ServiceResponse<ReservationDTO>.ResponseSuccess(
                    dto.ReservationInfoDto.ReservationId == 0 ? "New reservation added successfully" : "Reservation updated successfully",
                    Data: dto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                var message = ex.InnerException?.Message ?? ex.Message;
                return ServiceResponse<ReservationDTO>.ResponseFailure($"Reservation failed: {message}");
            }
        }

        public async Task<ServiceResponse<string>> QuickCheckInByIdAsync(int Id, int UserId)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(Id, includes: r => r.ReservationsRooms);
            if (reservation == null)
                return ServiceResponse<string>.ResponseFailure("No reservation found!");
            
            if(reservation.Status == ReservationStatus.CheckedIn)
                return ServiceResponse<string>.ResponseFailure("Reservation already Checked In.");
            
            if (reservation.Status == ReservationStatus.CheckedOut)
                return ServiceResponse<string>.ResponseFailure("Cannot check in this reservation, because It is Checked Out.");
            
            if (reservation.Status == ReservationStatus.Cancelled)
                return ServiceResponse<string>.ResponseFailure("Cannot check in this reservation, because It is Cancelled.");

            var selectedRoomsCount = await _unitOfWork.ReservationRoomRepository
                .CountAsync(rr => rr.ReservationId == reservation.Id);

            var requiredRoomsCount = await _unitOfWork.ReservationRoomTypeRepository
                .SumAsync(rrt => rrt.ReservationId == reservation.Id, rrt => rrt.Quantity);

            if (selectedRoomsCount < requiredRoomsCount)
            {
                return ServiceResponse<string>.ResponseFailure("Not all rooms have been selected for this reservation.");
            }


            try
            {
                reservation.Status = ReservationStatus.CheckedIn;
                await AddReservationHistoryAsync(Id, ReservationStatus.CheckedIn, UserId);

                _unitOfWork.ReservationRepository.Update(reservation);

                var RoomsIDs = reservation.ReservationsRooms.Select(r => r.RoomId).ToList();
                var checkedInRooms = await _unitOfWork.RoomRepository.GetAllAsync(r => RoomsIDs.Contains(r.Id));

                var systemSettings = await _unitOfWork.SystemSettingRepository.FirstOrDefaultAsync();

                foreach (var room in checkedInRooms)
                {
                    room.RoomStatusId = systemSettings.CheckInStatusId;
                }
                _unitOfWork.RoomRepository.UpdateRange(checkedInRooms);

                await _unitOfWork.CommitAsync();
                return ServiceResponse<string>.ResponseSuccess($"Reservation #{reservation.ReservationNumber} is Checked In.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<string>.ResponseFailure(ex.Message);
            }
        }

        public async Task<CheckOutDTO> GetCheckOutReservationByIdAsync(int Id)
        {
            var dto = await _unitOfWork.ReservationRepository.GetCheckOutReservationByIdAsync(Id);

            return dto;
        }

        public async Task<ServiceResponse<CheckOutDTO>> CheckOutReservationAsync(CheckOutDTO dto, int UserId)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(dto.Id, includes: r => r.ReservationsRooms);
            if (reservation == null)
            {
                return ServiceResponse<CheckOutDTO>.ResponseFailure("Reservation not found!");
            }
            if(reservation.Status != ReservationStatus.CheckedIn)
            {
				return ServiceResponse<CheckOutDTO>.ResponseFailure("Reservation must be Checked In Before Checked Out!");
			}
            try
            {
                reservation.Status = ReservationStatus.CheckedOut;
                _unitOfWork.ReservationRepository.Update(reservation);

                await AddReservationHistoryAsync(reservation.Id, ReservationStatus.CheckedOut, UserId);

                var RoomsIDs = reservation.ReservationsRooms.Select(r => r.RoomId).ToList();
                var checkedOutRooms = await _unitOfWork.RoomRepository.GetAllAsync(r => RoomsIDs.Contains(r.Id));
                var systemSettings = await _unitOfWork.SystemSettingRepository.FirstOrDefaultAsync();
                
                foreach(var room  in checkedOutRooms)
                {
                    room.RoomStatusId = systemSettings.CheckOutStatusId;
                }
                _unitOfWork.RoomRepository.UpdateRange(checkedOutRooms);

                await _unitOfWork.CommitAsync();   

                return ServiceResponse<CheckOutDTO>.ResponseSuccess("Reservation Checked Out Successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<CheckOutDTO>.ResponseFailure(ex.Message);
            }
        }

        #region Helper Methods
        private IEnumerable<ReservationStatus> ExtractStatuses(ConfirmReservationDTO dto)
        {
            var statuses = new List<ReservationStatus>();
            if (dto.IsPending) statuses.Add(ReservationStatus.Pending);
            if (dto.IsConfirmed) statuses.Add(ReservationStatus.Confirmed);
            if (dto.IsCheckedIn) statuses.Add(ReservationStatus.CheckedIn);
            if (dto.IsCheckedOut) statuses.Add(ReservationStatus.CheckedOut);
            if (dto.IsCancelled) statuses.Add(ReservationStatus.Cancelled);
            if (dto.IsNoShow) statuses.Add(ReservationStatus.NoShow);
            return statuses;
        }
        private async Task AddReservationHistoryAsync(int reservationId, ReservationStatus status, int? userId)
		{
			int userID = userId.Value;
			var reservationHistory = new ReservationHistory
			{
				ReservationId = reservationId,
				Status = status,
				PerformedById = userID,
				PerformedDate = DateTime.UtcNow
			};

			await _unitOfWork.ReservationHistoryRepository.AddNewAsync(reservationHistory);
			//await _unitOfWork.CommitAsync();
		}

		private async Task AddReservationHistoryRangeAsync(int reservationId, IEnumerable<ReservationStatus> statuses, int? userId)
		{
			int userID = userId.Value;
			var histories = statuses.Select(status => new ReservationHistory
			{
				ReservationId = reservationId,
				Status = status,
				PerformedById = userID,
				PerformedDate = DateTime.UtcNow
			});

			await _unitOfWork.ReservationHistoryRepository.AddRangeAsync(histories);
            //await _unitOfWork.CommitAsync();
        }
        #endregion
    }
}

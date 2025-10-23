using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.DTOs.RoomStatus;
using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Application.Interfaces;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Application.Services.CurrentUserService;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using HotelApp.Domain.Enums;

namespace HotelApp.Application.Services.ReservationService
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public ReservationService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
            _currentUserService = currentUserService;
        }
		public async Task<IEnumerable<GetAllReservationsDTO>> GetAllReservationAsync()
        {
			var reservation = await _unitOfWork.ReservationRepository
				.GetAllAsDtoAsync<GetAllReservationsDTO>();

			return reservation;
		}
		public async Task<GetReservationDetailsByIdDTO> GetReservationDetailsByIdAsync(int Id)
		{
			//var reservation = await _unitOfWork.ReservationRepository.GetReservationDetailsByIds(Id);

			var reservation = await _unitOfWork.ReservationRepository.GetReservationDetailsByIdAsync(Id);
			return reservation;
		}
		public async Task<IEnumerable<GetAllReservationsDTO>> GetFilteredReservationsAsync(ReservationFilterDTO dto)
		{
			var reservations = await _unitOfWork.ReservationRepository.GetFilteredReservationsAsync(dto);

			return reservations;
		}
        public async Task<ServiceResponse<ReservationDTO>> SaveReservation(ReservationDTO dto, int UserId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                /* Will Be Calculated */
                decimal totalPrice = 100;
                decimal totalRatePerNight = 100;

                var statuses = ExtractStatuses(dto.ConfirmDto);

                Reservation? reservation = new Reservation();

                if (dto.ReservationInfoDto.ReservationId == 0)
                {
                    // ===== Add New Reservation =====
                    reservation = _mapper.Map<Reservation>(dto);
                    reservation.PricePerNight = totalRatePerNight;
                    reservation.TotalPrice = totalPrice;
                    reservation.ReservationNumber = await _unitOfWork.ReservationRepository.GenerateReservationNumberAsync(2);
                    if (statuses != null && statuses.Any())
                        reservation.Status = statuses.Last();

                    await _unitOfWork.ReservationRepository.AddNewAsync(reservation);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    // ===== Edit Existing Reservation =====
                    reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(dto.ReservationInfoDto.ReservationId);
                    if (reservation == null)
                        return ServiceResponse<ReservationDTO>.ResponseFailure("Reservation not found");

                    _mapper.Map(dto, reservation);
                    reservation.PricePerNight = totalRatePerNight;
                    reservation.TotalPrice = totalPrice;
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
                if (dto.ReservationInfoDto.RoomsIDs.Any())
                {
                    var reservationRooms = dto.ReservationInfoDto.RoomsIDs.Select(rr => new ReservationRoom
                    {
                        ReservationId = reservation.Id,
                        RoomId = rr,
                        StartDate = reservation.CheckInDate,
                        EndDate = reservation.CheckOutDate
                    });
                    await _unitOfWork.ReservationRoomRepository.AddRangeAsync(reservationRooms);

                    var rooms = await _unitOfWork.RoomRepository
                        .GetAllAsync(r => dto.ReservationInfoDto.RoomsIDs.Contains(r.Id));

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

                await AddReservationHistoryRangeAsync(reservation.Id, statuses, UserId);

                await _unitOfWork.CommitTransactionAsync();

                return ServiceResponse<ReservationDTO>.ResponseSuccess(
                    dto.ReservationInfoDto.ReservationId == 0 ? "New reservation added successfully" : "Reservation updated successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                var message = ex.InnerException?.Message ?? ex.Message;
                return ServiceResponse<ReservationDTO>.ResponseFailure($"Reservation failed: {message}");
            }
        }

        //public async Task<ServiceResponse<ReservationDTO>> AddReservation(ReservationDTO dto, int UserId)
        //{
        //	try
        //	{
        //		await _unitOfWork.BeginTransactionAsync();

        //		/* Will Be Calculated */
        //		decimal totalPrice = 100;
        //		decimal totalRatePerNight = 100;

        //		var statuses = ExtractStatuses(dto.ConfirmDto);
        //		//if (dto.ConfirmDto.IsPending)
        //		//	statuses.Add(ReservationStatus.Pending);
        //		//if (dto.ConfirmDto.IsConfirmed)
        //		//	statuses.Add(ReservationStatus.Confirmed);
        //		//if (dto.ConfirmDto.IsCheckedIn)
        //		//	statuses.Add(ReservationStatus.CheckedIn);
        //		//if (dto.ConfirmDto.IsCheckedOut)
        //		//	statuses.Add(ReservationStatus.CheckedOut);
        //		//if (dto.ConfirmDto.IsCancelled)
        //		//	statuses.Add(ReservationStatus.Cancelled);
        //		//if (dto.ConfirmDto.IsNoShow)
        //		//	statuses.Add(ReservationStatus.NoShow);

        //		var reservation = _mapper.Map<Reservation>(dto);
        //		reservation.PricePerNight = totalRatePerNight;
        //		reservation.TotalPrice = totalPrice;

        //		if (statuses != null && statuses.Any())
        //		{
        //			reservation.Status = statuses.Last();
        //		}

        //		await _unitOfWork.ReservationRepository.AddNewAsync(reservation);
        //		await _unitOfWork.CommitAsync();

        //		var reservationRoomTypes = dto.ReservationInfoDto.RoomTypeToBookDTOs.Select(rt => new ReservationRoomType
        //		{
        //			RoomTypeId = rt.RoomTypeId,
        //			ReservationId = reservation.Id,
        //			Quantity = rt.NumOfRooms,
        //			NumOfAdults = rt.NumOfAdults,
        //			NumOfChildren = rt.NumOfChildrens
        //		}).ToList();
        //		await _unitOfWork.ReservationRoomTypeRepository.AddRangeAsync(reservationRoomTypes);

        //		if (dto.ReservationInfoDto.RoomsIDs.Any())
        //		{
        //			var reservationRooms = dto.ReservationInfoDto.RoomsIDs.Select(rr => new ReservationRoom
        //			{
        //				ReservationId = reservation.Id,
        //				RoomId = rr,
        //				StartDate = reservation.CheckInDate,
        //				EndDate = reservation.CheckOutDate
        //			});
        //			await _unitOfWork.ReservationRoomRepository.AddRangeAsync(reservationRooms);

        //			var rooms = await _unitOfWork.RoomRepository
        //				.GetAllAsync(r => dto.ReservationInfoDto.RoomsIDs.Contains(r.Id));

        //			if (dto.ConfirmDto.IsCheckedIn)
        //			{
        //				var systemSettings = await _unitOfWork.SystemSettingRepository.FirstOrDefaultAsync();
        //				foreach (var room in rooms)
        //				{
        //					room.RoomStatusId = systemSettings.CheckInStatusId;
        //				}
        //			}
        //			else if(dto.ConfirmDto.IsConfirmed || dto.ConfirmDto.IsPending && !dto.ConfirmDto.IsCheckedIn)
        //			{
        //				var roomStatus = await _unitOfWork.RoomStatusRepository.FirstOrDefaultAsync(rs => rs.Code == RoomStatusEnum.Reserved, SkipBranchFilter: true);
        //				foreach (var room in rooms)
        //				{
        //					room.RoomStatusId = roomStatus.Id;
        //				}
        //			}
        //			_unitOfWork.RoomRepository.UpdateRange(rooms);
        //		}

        //		var guestReservations = dto.GuestDtos.Select((guest, index) => new GuestReservation
        //		{
        //			GuestId = guest.GuestId,
        //			ReservationId = reservation.Id,
        //			IsPrimaryGuest = dto.GuestDtos[index].IsPrimary
        //		}).ToList();
        //		await _unitOfWork.GuestReservationRepository.AddRangeAsync(guestReservations);

        //              await AddReservationHistoryRangeAsync(reservation.Id, statuses, UserId);

        //              await _unitOfWork.CommitTransactionAsync();

        //		return ServiceResponse<ReservationDTO>.ResponseSuccess("New reservation added successfully");
        //	}
        //	catch (Exception ex)
        //	{
        //		 await _unitOfWork.RollbackTransactionAsync();
        //		var message = ex.InnerException?.Message ?? ex.Message;
        //		return ServiceResponse<ReservationDTO>.ResponseFailure($"Reservation failed: {message}");
        //	}
        //}

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
        public async Task<ReservationDTO?> GetReservationToEditByIdAsync(int Id)
		{
			var dto = await _unitOfWork.ReservationRepository.GetReservationToEditByIdAsync(Id);

			return dto;
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

using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.DTOs.RoomStatus;
using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Application.Interfaces;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using HotelApp.Domain.Enums;

namespace HotelApp.Application.Services.ReservationService
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

        public ReservationService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
        }

		public async Task<IEnumerable<GetAllReservationsDTO>> GetAllReservationAsync()
        {
			var reservation = await _unitOfWork.ReservationRepository
				.GetAllAsDtoAsync<GetAllReservationsDTO>();

			return reservation;
		}
		public async Task<Reservation> GetReservationDetailsByIdAsync(int Id)
		{
			var reservation = await _unitOfWork.ReservationRepository.GetReservationDetailsByIds(Id);

			return reservation;
		}
		public async Task<ServiceResponse<ReservationDTO>> AddReservation(ReservationDTO dto)
		{
			try
			{
				await _unitOfWork.BeginTransactionAsync();
				var roomTypeIds = dto.bookRoomDTO.RoomTypeToBookDTOs.Select(rt => rt.RoomTypeId).ToList();
				var roomTypes = await _unitOfWork.RoomTypeRepository.GetAllAsync(rt => roomTypeIds.Contains(rt.Id));

				decimal totalPrice = 0;
				decimal totalRatePerNight = 0;

				foreach (var roomType in roomTypes)
				{
					var roomTypeDto = dto.bookRoomDTO.RoomTypeToBookDTOs.FirstOrDefault(dto => dto.RoomTypeId == roomType.Id);
					if (roomTypeDto != null)
					{
						totalRatePerNight += roomType.PricePerNight * roomTypeDto.NumOfRooms;
						totalPrice += roomType.PricePerNight * roomTypeDto.NumOfRooms * dto.bookRoomDTO.NumOfNights;
					}
				}

				// Save Reservation
				var reservation = _mapper.Map<Reservation>(dto);
				reservation.PricePerNight = totalRatePerNight;
				reservation.TotalPrice = totalPrice;

				await _unitOfWork.ReservationRepository.AddNewAsync(reservation);
				await _unitOfWork.CommitAsync();

				// Add RoomType
				var reservationRoomTypes = dto.bookRoomDTO.RoomTypeToBookDTOs.Select(rt => new ReservationRoomType
				{
					RoomTypeId = rt.RoomTypeId,
					ReservationId = reservation.Id,
					Quantity = rt.NumOfRooms,
					NumOfAdults = rt.NumOfAdults,
					NumOfChildren = rt.NumOfChildrens
				}).ToList();

				await _unitOfWork.ReservationRoomTypeRepository.AddRangeAsync(reservationRoomTypes);

				// Add Rooms
				if (dto.bookRoomDTO.RoomsIDs.Any())
				{
					var reservationRooms = dto.bookRoomDTO.RoomsIDs.Select(rr => new ReservationRoom
					{
						ReservationId = reservation.Id,
						RoomId = rr,
						StartDate = reservation.CheckInDate,
						EndDate = reservation.CheckOutDate
					});
					await _unitOfWork.ReservationRoomRepository.AddRangeAsync(reservationRooms);

					var rooms = await _unitOfWork.RoomRepository
						.GetAllAsync(r => dto.bookRoomDTO.RoomsIDs.Contains(r.Id));

					if (dto.confirmDTO.IsCheckedIn)
					{
						var systemSettings = await _unitOfWork.SystemSettingRepository.FirstOrDefaultAsync();
						foreach (var room in rooms)
						{
							room.RoomStatusId = systemSettings.CheckInStatusId;
						}
					}
					else if(dto.confirmDTO.IsConfirmed || dto.confirmDTO.IsPending && !dto.confirmDTO.IsCheckedIn)
					{
						var roomStatus = await _unitOfWork.RoomStatusRepository.FirstOrDefaultAsync(rs => rs.Code == RoomStatusEnum.Reserved, SkipBranchFilter: true);
						foreach (var room in rooms)
						{
							room.RoomStatusId = roomStatus.Id;
						}
					}
					_unitOfWork.RoomRepository.UpdateRange(rooms);
				}


				// Link Guests to Reservation
				var guestReservations = dto.GuestDTOs.Select((guest, index) => new GuestReservation
				{
					GuestId = guest.GuestId,
					ReservationId = reservation.Id,
					IsPrimaryGuest = dto.GuestDTOs[index].IsPrimary
				}).ToList();

				await _unitOfWork.GuestReservationRepository.AddRangeAsync(guestReservations);
				await _unitOfWork.CommitTransactionAsync();

				return ServiceResponse<ReservationDTO>.ResponseSuccess("New reservation added successfully");
			}
			catch (Exception ex)
			{
				 await _unitOfWork.RollbackTransactionAsync();
				var message = ex.InnerException?.Message ?? ex.Message;
				return ServiceResponse<ReservationDTO>.ResponseFailure($"Reservation failed: {message}");
			}
		}

		public async Task<IEnumerable<GetAllReservationsDTO>> GetFilteredReservationsAsync(ReservationFilterDTO dto)
		{
			var reservations = await _unitOfWork.ReservationRepository.GetFilteredReservationsAsync(dto);

			return reservations;
		}

	}
}

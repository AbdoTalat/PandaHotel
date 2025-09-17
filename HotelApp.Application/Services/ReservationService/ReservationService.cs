using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.DTOs.RoomStatus;
using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Application.IRepositories;
using HotelApp.Domain;
using HotelApp.Domain.Entities;

namespace HotelApp.Application.Services.ReservationService
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReservationRepository _reservationRepository;
		private readonly IMapper _mapper;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public ReservationService(IUnitOfWork unitOfWork, IReservationRepository reservationRepository,
             IMapper mapper, IRoomTypeRepository roomTypeRepository)
        {
            _unitOfWork = unitOfWork;
            _reservationRepository = reservationRepository;
			_mapper = mapper;
            _roomTypeRepository = roomTypeRepository;
        }

		public async Task<IEnumerable<GetAllReservationsDTO>> GetAllReservationAsync()
        {
			var reservation = await _unitOfWork.Repository<Reservation>()
				.GetAllAsDtoAsync<GetAllReservationsDTO>();

			return reservation;
		}

		public async Task<ServiceResponse<ReservationDTO>> AddReservation(ReservationDTO dto)
		{
			try
			{
				await _unitOfWork.BeginTransactionAsync();
				var roomTypeIds = dto.bookRoomDTO.roomTypeToBookDTOs.Select(rt => rt.Id).ToList();
				var roomTypes = await _unitOfWork.Repository<RoomType>().GetAllAsync(rt => roomTypeIds.Contains(rt.Id));

				decimal totalPrice = 0;
				decimal totalRatePerNight = 0;

				foreach (var roomType in roomTypes)
				{
					var roomTypeDto = dto.bookRoomDTO.roomTypeToBookDTOs.FirstOrDefault(dto => dto.Id == roomType.Id);
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

				await _unitOfWork.Repository<Reservation>().AddNewAsync(reservation);
				await _unitOfWork.CommitAsync();

				// Add RoomType
				var reservationRoomTypes = dto.bookRoomDTO.roomTypeToBookDTOs.Select(rt => new ReservationRoomType
				{
					RoomTypeId = rt.Id,
					ReservationId = reservation.Id,
					Quantity = rt.NumOfRooms,
					NumOfAdults = rt.NumOfAdults,
					NumOfChildren = rt.NumOfChildren
				}).ToList();

				await _unitOfWork.Repository<ReservationRoomType>().AddRangeAsync(reservationRoomTypes);

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
					await _unitOfWork.Repository<ReservationRoom>().AddRangeAsync(reservationRooms);

					var rooms = await _unitOfWork.Repository<Room>()
						.GetAllAsync(r => dto.bookRoomDTO.RoomsIDs.Contains(r.Id));

					if (dto.confirmDTO.IsCheckedIn)
					{
						var systemSettings = await _unitOfWork.Repository<SystemSetting>().FirstOrDefaultAsync();
						foreach (var room in rooms)
						{
							room.RoomStatusId = systemSettings.CheckInStatusId;
						}
					}
					else if(dto.confirmDTO.IsConfirmed || dto.confirmDTO.IsPending && !dto.confirmDTO.IsCheckedIn)
					{
						var roomStatus = await _unitOfWork.Repository<RoomStatus>().FirstOrDefaultAsDtoAsync<DropDownDTO<string>>(rs => rs.Name == "Reserved");
						foreach (var room in rooms)
						{
							room.RoomStatusId = roomStatus.Id;
						}
					}
					_unitOfWork.Repository<Room>().UpdateRange(rooms);
				}


				// Link Guests to Reservation
				var guestReservations = dto.GuestDTOs.Select((guest, index) => new GuestReservation
				{
					GuestId = guest.GuestId,
					ReservationId = reservation.Id,
					IsPrimaryGuest = dto.GuestDTOs[index].IsPrimary
				}).ToList();

				await _unitOfWork.Repository<GuestReservation>().AddRangeAsync(guestReservations);
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
			var reservations = await _reservationRepository.GetFilteredReservationsAsync(dto);

			return reservations;
		}

	}
}

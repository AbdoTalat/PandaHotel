using AutoMapper;
using HotelApp.Application.DTOs.Reservation;
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
			var reservation = await _unitOfWork.Repository<Reservation>().GetAllAsDtoAsync<GetAllReservationsDTO>();

			return reservation;
		}

		public async Task<ServiceResponse<AddReservationDTO>> AddReservation(AddReservationDTO reservationDTO, int userId)
		{
			try
			{
				var roomTypeIds = reservationDTO.bookRoomDTO.roomTypeToBookDTOs.Select(rt => rt.Id).ToList();
				var roomTypes = await _roomTypeRepository.GetRoomTypesByIDsAsync(roomTypeIds);

				var checkIn = reservationDTO.bookRoomDTO.CheckInDate;
				var checkOut = reservationDTO.bookRoomDTO.CheckOutDate;

				decimal totalPrice = 0;
				decimal totalRatePerNight = 0;

				foreach (var roomType in roomTypes)
				{
					var matchingDto = reservationDTO.bookRoomDTO.roomTypeToBookDTOs
						.FirstOrDefault(dto => dto.Id == roomType.Id);

					if (matchingDto != null)
					{
						int numOfRooms = matchingDto.NumOfRooms;
						totalRatePerNight += roomType.PricePerNight * numOfRooms;
						totalPrice += roomType.PricePerNight * numOfRooms * reservationDTO.bookRoomDTO.NumOfNights;
					}
				}

				// Save Reservation
				var reservation = _mapper.Map<Reservation>(reservationDTO);
				reservation.PricePerNight = totalRatePerNight;
				reservation.TotalPrice = totalPrice;

				await _unitOfWork.Repository<Reservation>().AddNewAsync(reservation);
				await _unitOfWork.CommitAsync();

				// Add RoomType
				var reservationRoomTypes = reservationDTO.bookRoomDTO.roomTypeToBookDTOs.Select(rt => new ReservationRoomType
				{
					RoomTypeId = rt.Id,
					ReservationId = reservation.Id,
					Quantity = rt.NumOfRooms,
					NumOfAdults = rt.NumOfAdults,
					NumOfChildren = rt.NumOfChildren
				}).ToList();

				await _unitOfWork.Repository<ReservationRoomType>().AddRangeAsync(reservationRoomTypes);
				await _unitOfWork.CommitAsync();


				// Link Guests to Reservation
				var guestReservations = reservationDTO.GuestsDTOs.Select((guest, index) => new GuestReservation
				{
					GuestId = guest.GuestId,
					ReservationId = reservation.Id,
					IsPrimaryGuest = reservationDTO.GuestsDTOs[index].IsPrimary
				}).ToList();

				await _unitOfWork.Repository<GuestReservation>().AddRangeAsync(guestReservations);
				await _unitOfWork.CommitAsync();

				return ServiceResponse<AddReservationDTO>.ResponseSuccess("New reservation added successfully");
			}
			catch (Exception ex)
			{
				var message = ex.InnerException?.Message ?? ex.Message;
				return ServiceResponse<AddReservationDTO>.ResponseFailure($"Reservation failed: {message}");
			}
		}


		
	}
}

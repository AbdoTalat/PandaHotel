using AutoMapper;
using HotelApp.Application.Interfaces;
using Microsoft.Extensions.Logging;
using HotelApp.Application.DTOs.Rooms;
using HotelApp.Application.Services.RoomStatusService;
using HotelApp.Application.Services.RoomTypeService;
using Microsoft.EntityFrameworkCore;
using HotelApp.Application.DTOs.RoleBased;
using HotelApp.Domain.Entities;
using BenchmarkDotNet.Running;
using System.Web.Mvc;
using HotelApp.Helper;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.Interfaces.IRepositories;

namespace HotelApp.Application.Services.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IRoomRepository _roomRepository;
		private readonly IMapper _mapper;
		private readonly IRoomTypeService _roomTypeService;

		public RoomService(IUnitOfWork unitOfWork, IRoomRepository roomRepository, IMapper mapper,
             IRoomTypeService roomTypeService)
        {
            _unitOfWork = unitOfWork;
			_roomRepository = roomRepository;
			_mapper = mapper;
			_roomTypeService = roomTypeService;
		}


		public async Task<IEnumerable<GetAllRoomsDTO>> GetAllRoomsAsync()
        {
            var rooms = await _unitOfWork.RoomRepository.GetAllAsDtoAsync<GetAllRoomsDTO>();
            
            
            return rooms;
        }
		public GetRoomsReview GetRoomsReview()
		{
            var result = _roomRepository.GetRoomsReview();

            return result;
        }
		public async Task<EditRoomDTO?> GetRoomToEditByIdAsync(int Id)
        {
            var room = await _unitOfWork.RoomRepository
                .GetByIdAsDtoAsync<EditRoomDTO>(Id);
            
            return room;
        }
		public async Task<GetRoomByIdDTO?> GetRoomByIdAsync(int Id)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsDtoAsync<GetRoomByIdDTO>(Id);
            return room;
        }

        public async Task<ServiceResponse<AddRoomDTO>> AddRoomAsync(AddRoomDTO roomDTO)
        {
            try
            {
                bool IsRoomNumberUnique = await _unitOfWork.RoomRepository
                    .IsExistsAsync(r => r.RoomNumber.ToLower() == roomDTO.RoomNumber.ToLower());
                if (IsRoomNumberUnique)
                {
                    return ServiceResponse<AddRoomDTO>.ResponseFailure("Cannot enter duplicate room number.");
                }
                var mappedRoom = _mapper.Map<Room>(roomDTO);
                await _unitOfWork.RoomRepository.AddNewAsync(mappedRoom);
                await _unitOfWork.CommitAsync();

                if (roomDTO.SelectedOptions != null)
                {
                    var roomOptions = roomDTO.SelectedOptions.Select(optionId => new RoomOption
                    {
                        RoomId = mappedRoom.Id,
                        OptionId = optionId
                    });

                    await _unitOfWork.RoomOptionRepository.AddRangeAsync(roomOptions);

                }
                await _unitOfWork.CommitAsync();

                return ServiceResponse<AddRoomDTO>.ResponseSuccess("New Room Created Successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<AddRoomDTO>.ResponseFailure(ex.InnerException.Message);
            }
        }
		public async Task<ServiceResponse<AddRoomDTO>> AddManyRoomsAsync(AddRoomDTO dto)
		{
            if(dto.RoomNumberTo > 100)
            {
                return ServiceResponse<AddRoomDTO>.ResponseFailure("Can't add more than 100 rooms in one operation.");
            }
			if (dto.RoomNumberFrom < 1 || dto.RoomNumberTo < 1)
			{
				return ServiceResponse<AddRoomDTO>.ResponseFailure("Numbers Must be positive.");
			}
			try
			{
				var roomsToAdd = new List<Room>();

				for (int? i = dto.RoomNumberFrom; i <= dto.RoomNumberTo; i++)
				{
					var roomNumber = string.IsNullOrEmpty(dto.RoomNumberText)
				        ? i.ToString()
				        : $"{dto.RoomNumberText}{i}";

					bool isRoomNumberExist = await _unitOfWork.RoomRepository
						.IsExistsAsync(r => r.RoomNumber.ToLower() == roomNumber.ToLower());

					if (isRoomNumberExist)
					{
						continue;
					}

					var room = new Room
					{
						RoomNumber = roomNumber,
						Description = dto.Description,
						FloorId = dto.FloorId,
						RoomStatusId = dto.RoomStatusId,
						IsActive = dto.IsActive,
						IsAffectedByRoomType = dto.IsAffectedByRoomType,
						PricePerNight = dto.PricePerNight,
						MaxNumOfAdults = dto.MaxNumOfAdults,
						MaxNumOfChildrens = dto.MaxNumOfChildrens,
						RoomTypeId = dto.RoomTypeId,
                        RoomOptions = dto.SelectedOptions?.Select(optionId => new RoomOption
                        {
                            OptionId = optionId
                        }).ToList()
                    };
					

					roomsToAdd.Add(room);
				}

				if (roomsToAdd.Count > 0)
				{
					await _unitOfWork.RoomRepository.AddRangeAsync(roomsToAdd);
					await _unitOfWork.CommitAsync();
					return ServiceResponse<AddRoomDTO>.ResponseSuccess( $"{roomsToAdd.Count} room(s) added successfully.");
				}

				return ServiceResponse<AddRoomDTO>.ResponseFailure("No rooms were added. All specified room numbers already exist.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<AddRoomDTO>.ResponseFailure(ex.Message);
			}
		}


		public async Task<ServiceResponse<EditRoomDTO>> EditRoomAsync(EditRoomDTO room)
        {
            var oldRoom = await _unitOfWork.RoomRepository.GetByIdAsync(room.Id);
            if (oldRoom == null)
            {
                return ServiceResponse<EditRoomDTO>.ResponseFailure("Room Is Not Found.");
            }
            try
            {
				bool IsRoomNumberExist = await _unitOfWork.RoomRepository
                    .IsExistsAsync(r => r.RoomNumber.ToLower() ==  room.RoomNumber.ToLower() && r.Id !=room.Id && r.BranchId == oldRoom.BranchId);
				if (IsRoomNumberExist)
				{
					return ServiceResponse<EditRoomDTO>.ResponseFailure("Cannot enter duplicate room number.");
				}

                _mapper.Map(room, oldRoom);

                _unitOfWork.RoomRepository.Update(oldRoom);

                var oldRoomOption = await _unitOfWork.RoomOptionRepository.GetAllAsync(ro => ro.RoomId == room.Id);

                _unitOfWork.RoomOptionRepository.DeleteRange(oldRoomOption);

                if (room.SelectedOptions != null)
                {
                    var roomOptions = room.SelectedOptions.Select(optionId => new RoomOption
                    {
                        RoomId = room.Id,
                        OptionId = optionId
                    });

                    await _unitOfWork.RoomOptionRepository.AddRangeAsync(roomOptions);
                    
                }
                await _unitOfWork.CommitAsync();
				return ServiceResponse<EditRoomDTO>.ResponseSuccess("Room Updated Successfully.");

            }
            catch (Exception ex)
            {
                return ServiceResponse<EditRoomDTO>.ResponseFailure($"Error Occurred: {ex.InnerException.Message}");
            }
        }

        public async Task<ServiceResponse<Room>> DeleteRoomAsync(int Id)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(Id);
            if (room == null)
            {
                return ServiceResponse<Room>.ResponseFailure($"There is no room with this ID: {Id}");
            }

            bool isRoomReserved = await _unitOfWork.ReservationRoomRepository.IsExistsAsync(r => r.RoomId == Id);
            if (isRoomReserved)
            {
                return ServiceResponse<Room>.ResponseFailure("Cannot delete room that is already reserved before.");
            }
            try
            {
                _unitOfWork.RoomRepository.Delete(room);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<Room>.ResponseSuccess("Room Deleted Successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Room>.ResponseFailure($"Error Occurred While Deleting This Room: {ex.Message}");
            }
        }

		public async Task<RoomReportDTO> GetRoomsReportBetweenDatesAsync(DateTime start, DateTime end)
        {
            var roomsData = await _roomRepository.GetRoomsReportBetweenDatesAsync(start, end);

            return roomsData;
        }
        public async Task<IEnumerable<GetAvailableRoomsDTO>> GetAvailableRoomsAsync(string? name)
        {
            var rooms = await _unitOfWork.RoomRepository
                .GetAllAsDtoAsync<GetAvailableRoomsDTO>(r => r.RoomNumber.Contains(name) && r.IsActive && r.RoomStatus.IsReservable);

            return rooms;
        }
        public async Task<IEnumerable<GetAllRoomsDTO>> GetFilteredRoomsAsync(RoomFilterDTO dto)
        {
            var result = await _roomRepository.GetFilteredRoomsAsync(dto);

            return result;
		}

        public async Task<ServiceResponse<object>> ValidateRoomSelectionsAsync(List<RoomTypeToBookDTO> roomTypeToBookDTOs, List<int> selectedRoomIds)
        {
            var result = await _roomRepository.ValidateRoomSelectionsAsync(roomTypeToBookDTOs, selectedRoomIds);
            return result;
        }

        public async Task<int> GetOccupancyPercentAsync()
        {
            var result = await _roomRepository.GetOccupancyPercentAsync();
            return result;
        }
	}
}

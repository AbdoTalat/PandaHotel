using AutoMapper;
using HotelApp.Domain;
using Microsoft.Extensions.Logging;
using HotelApp.Application.DTOs.Rooms;
using HotelApp.Application.Services.RoomStatusService;
using HotelApp.Application.Services.RoomTypeService;
using Microsoft.EntityFrameworkCore;
using HotelApp.Application.DTOs.RoleBased;
using HotelApp.Application.IRepositories;
using HotelApp.Domain.Entities;
using BenchmarkDotNet.Running;
using System.Web.Mvc;
using HotelApp.Helper;

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
            var rooms = await _unitOfWork.Repository<Room>().GetAllAsDtoAsync<GetAllRoomsDTO>();
            
            
            return rooms;
        }
		public GetRoomsReview GetRoomsReview()
		{
            var result = _roomRepository.GetRoomsReview();

            return result;
        }
		public async Task<EditRoomDTO?> GetRoomToEditByIdAsync(int Id)
        {
            var room = await _unitOfWork.Repository<Room>()
                .GetByIdAsDtoAsync<EditRoomDTO>(Id);
            
            return room;
        }
		public async Task<GetRoomByIdDTO?> GetRoomByIdAsync(int Id)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsDtoAsync<GetRoomByIdDTO>(Id);
            return room;
        }

        public async Task<ServiceResponse<AddRoomDTO>> AddRoomAsync(AddRoomDTO roomDTO)
        {
            try
            {
                bool IsRoomNumberUnique = await _unitOfWork.Repository<Room>()
                    .IsExistsAsync(r => r.RoomNumber.ToLower() == roomDTO.RoomNumber.ToLower() && r.BranchId == roomDTO.BranchId);
                if (IsRoomNumberUnique)
                {
                    return ServiceResponse<AddRoomDTO>.ResponseFailure("Cannot enter duplicate room number.");
                }
                var mappedRoom = _mapper.Map<Room>(roomDTO);
                await _unitOfWork.Repository<Room>().AddNewAsync(mappedRoom);
                await _unitOfWork.CommitAsync();

                if (roomDTO.SelectedOptions != null)
                {
                    var roomOptions = roomDTO.SelectedOptions.Select(optionId => new RoomOption
                    {
                        RoomId = mappedRoom.Id,
                        OptionId = optionId
                    });

                    await _unitOfWork.Repository<RoomOption>().AddRangeAsync(roomOptions);

                }
                await _unitOfWork.CommitAsync();

                var roomType = await _unitOfWork.Repository<RoomType>().GetByIdAsync(roomDTO.RoomTypeId);
                if (roomType == null)
                {
                    return ServiceResponse<AddRoomDTO>.ResponseFailure("Room Type not found");
                }
                if (!roomType.IsActive)
                {
                    roomType.IsActive = true;
                    _unitOfWork.Repository<RoomType>().Update(roomType);

                    await _unitOfWork.CommitAsync();
                }
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

					bool isRoomNumberExist = await _unitOfWork.Repository<Room>()
						.IsExistsAsync(r => r.RoomNumber.ToLower() == roomNumber.ToLower() && r.BranchId == dto.BranchId);

					if (isRoomNumberExist)
					{
						continue;
					}

					var room = new Room
					{
						RoomNumber = roomNumber,
						BranchId = dto.BranchId,
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
					await _unitOfWork.Repository<Room>().AddRangeAsync(roomsToAdd);
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
            var oldRoom = await _unitOfWork.Repository<Room>().GetByIdAsync(room.Id);
            if (oldRoom == null)
            {
                return ServiceResponse<EditRoomDTO>.ResponseFailure("Room Is Not Found.");
            }
            try
            {
				bool IsRoomNumberExist = await _unitOfWork.Repository<Room>()
                    .IsExistsAsync(r => r.RoomNumber.ToLower() ==  room.RoomNumber.ToLower() && r.Id !=room.Id && r.BranchId == oldRoom.BranchId);
				if (IsRoomNumberExist)
				{
					return ServiceResponse<EditRoomDTO>.ResponseFailure("Cannot enter duplicate room number.");
				}

                _mapper.Map(room, oldRoom);

                _unitOfWork.Repository<Room>().Update(oldRoom);

                var oldRoomOption = await _unitOfWork.Repository<RoomOption>().GetAllAsync(ro => ro.RoomId == room.Id);

                _unitOfWork.Repository<RoomOption>().DeleteRange(oldRoomOption);

                if (room.SelectedOptions != null)
                {
                    var roomOptions = room.SelectedOptions.Select(optionId => new RoomOption
                    {
                        RoomId = room.Id,
                        OptionId = optionId
                    });

                    await _unitOfWork.Repository<RoomOption>().AddRangeAsync(roomOptions);
                    
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
            try
            {
                var room = await _unitOfWork.Repository<Room>().GetByIdAsync(Id);
                if (room == null)
                {
					return ServiceResponse<Room>.ResponseFailure($"There is no room with this ID: {Id}");
				}
				//bool isRoomReserved = await _unitOfWork.Repository<Reservation>().IsExistsAsync(r => r.RoomId == Id);
				//if (isRoomReserved)
				//{
				//	return ServiceResponse<Room>.ResponseFailure("Cannot delete room that is already reserved");
				//}
				_unitOfWork.Repository<Room>().Delete(room);
				await _unitOfWork.CommitAsync();

				var roomType = await _unitOfWork.Repository<RoomType>().GetByIdAsync(room.RoomTypeId);
				if (roomType == null)
				{
					roomType.IsActive = false;
					_unitOfWork.Repository<RoomType>().Update(roomType);
                    await _unitOfWork.CommitAsync();
				}
				return ServiceResponse<Room>.ResponseSuccess("Room Deleted Successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Room>.ResponseFailure($"Error Occurred While Deleting This Room: {ex.Message}");
            }
        }
        public async Task<ServiceResponse<string>> CheckRoomAvailabilityAsync(int roomTypeId, int numberOfRooms)
        {
            var roomsCount = await _roomRepository.CheckRoomAvailabilityAsync(roomTypeId, numberOfRooms);
            var roomType = await _unitOfWork.Repository<RoomType>().GetByIdAsync(roomTypeId);

            if (roomsCount < numberOfRooms)
            {
                return ServiceResponse<string>.ResponseFailure($"Only {roomsCount} rooms available for the {roomType.Name} room type.");
            }

            return ServiceResponse<string>.ResponseSuccess("Rooms are available.");
        }

		public async Task<RoomReportDTO> GetRoomsReportBetweenDatesAsync(DateTime start, DateTime end)
        {
            var roomsData = await _roomRepository.GetRoomsReportBetweenDatesAsync(start, end);

            return roomsData;
        }

        public async Task<IEnumerable<GetAllRoomsDTO>> GetFilteredRoomsAsync(RoomFilterDTO dto)
        {
            //var result = await _unitOfWork.Repository<Room>()
            // .GetFilteredAsDtoAsync<GetAllRoomsDTO>(filter);

            var result = await _roomRepository.GetFilteredRoomsAsync(dto);

            return result;
		}
	}
}

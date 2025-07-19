using AutoMapper;
using HotelApp.Application.DTOs.RoomStatus;
using HotelApp.Application.IRepositories;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.RoomStatusService
{
    public class RoomStatusService : IRoomStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public RoomStatusService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
		}

        public async Task<IEnumerable<GetAllRoomStatusDTO>> GetAllRoomStatusAsync()
        {
            var roomStatus = await _unitOfWork.Repository<RoomStatus>().GetAllAsDtoAsync<GetAllRoomStatusDTO>();

            return roomStatus;
        }
		public async Task<IEnumerable<GetRoomStatusItemsDTO>> GetRoomStatusItemsAsync()
        {
            var roomStatus = await _unitOfWork.Repository<RoomStatus>().GetAllAsDtoAsync<GetRoomStatusItemsDTO>();
            
            return roomStatus;
        }
		public async Task<IEnumerable<GetRoomStatusItemsDTO>> GetRoomStatusItemsWithoutBranchFilterAsync()
        {
            var roomStatus = await _unitOfWork.Repository<RoomStatus>().GetAllAsDtoAsync<GetRoomStatusItemsDTO>(SkipBranchFilter:true);

            return roomStatus;
        }
		public async Task<EditRoomStatusDTO?> GetRoomStatusToEditByIdAsync(int Id)
        {
            var roomStatus = await _unitOfWork.Repository<RoomStatus>().GetByIdAsDtoAsync<EditRoomStatusDTO>(Id);

            return roomStatus;
        }
		public async Task<ServiceResponse<AddRoomStatusDTO>> AddRoomStatusAsync(AddRoomStatusDTO roomStatusDTO)
        {
            try
            {
                var roomStatusToAdd = _mapper.Map<RoomStatus>(roomStatusDTO);

                await _unitOfWork.Repository<RoomStatus>().AddNewAsync(roomStatusToAdd);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<AddRoomStatusDTO>.ResponseSuccess("New Room Status Created successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<AddRoomStatusDTO>.ResponseFailure(ex.Message);
            }
        }
		public async Task<ServiceResponse<EditRoomStatusDTO>> EditRoomStatusAsync(EditRoomStatusDTO roomStatusDTO)
        {
            var OldRoomStatus = await _unitOfWork.Repository<RoomStatus>().GetByIdAsync(roomStatusDTO.Id);
            if(OldRoomStatus == null)
            {
                return ServiceResponse<EditRoomStatusDTO>.ResponseFailure("Room Status not found.");
            }

            try
            {
                _mapper.Map(roomStatusDTO, OldRoomStatus);

                _unitOfWork.Repository<RoomStatus>().Update(OldRoomStatus);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<EditRoomStatusDTO>.ResponseSuccess("Room Status updated successfully.");
            }
            catch (Exception ex)
            {
				return ServiceResponse<EditRoomStatusDTO>.ResponseFailure(ex.Message);
			}
        }
	}
}

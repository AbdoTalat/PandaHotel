using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.RoomStatus;
using HotelApp.Application.IRepositories;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using HotelApp.Helper;
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
            int? branchId = BranchContext.CurrentBranchId;
            var roomStatus = await _unitOfWork.Repository<RoomStatus>()
                .GetAllAsDtoAsync<GetAllRoomStatusDTO>(rs => rs.IsSystem || rs.BranchId == branchId, 
                    SkipBranchFilter:true);

            return roomStatus;
        }
		public async Task<IEnumerable<DropDownDTO<string>>> GetRoomStatusDropDownAsync()
		{
			int? branchId = BranchContext.CurrentBranchId;
			var roomStatus = await _unitOfWork.Repository<RoomStatus>()
	        .GetAllAsDtoAsync<DropDownDTO<string>>(
		        rs => rs.IsSystem || rs.BranchId == branchId, SkipBranchFilter: true );

			return roomStatus;
        }
		public async Task<IEnumerable<DropDownDTO<string>>> GetRoomStatusDropDownWithoutBranchFilterAsync()
        {
            var roomStatus = await _unitOfWork.Repository<RoomStatus>()
                .GetAllAsDtoAsync<DropDownDTO<string>>(SkipBranchFilter:true);

            return roomStatus;
        }
		public async Task<RoomStatusDTO?> GetRoomStatusToEditByIdAsync(int Id)
        {
			int? branchId = BranchContext.CurrentBranchId;
			var roomStatus = await _unitOfWork.Repository<RoomStatus>()
			.GetByIdAsDtoAsync<RoomStatusDTO>(Id,
				rs => rs.IsSystem || rs.BranchId == branchId,
				SkipBranchFilter: true);

			return roomStatus;
        }
		public async Task<ServiceResponse<RoomStatusDTO>> AddRoomStatusAsync(RoomStatusDTO roomStatusDTO)
        {
            try
            {
                var roomStatusToAdd = _mapper.Map<RoomStatus>(roomStatusDTO);

                await _unitOfWork.Repository<RoomStatus>().AddNewAsync(roomStatusToAdd);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<RoomStatusDTO>.ResponseSuccess("New Room Status Created successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<RoomStatusDTO>.ResponseFailure(ex.Message);
            }
        }
		public async Task<ServiceResponse<RoomStatusDTO>> EditRoomStatusAsync(RoomStatusDTO dto)
        {
			int? branchId = BranchContext.CurrentBranchId;

			var OldRoomStatus = await _unitOfWork.Repository<RoomStatus>()
			.GetByIdAsync(dto.Id,
				rs => rs.IsSystem || rs.BranchId == branchId,SkipBranchFilter: true);

            if(OldRoomStatus == null)
            {
                return ServiceResponse<RoomStatusDTO>.ResponseFailure("Room Status not found.");
            }

            try
            {
                _mapper.Map(dto, OldRoomStatus);

                _unitOfWork.Repository<RoomStatus>().Update(OldRoomStatus);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<RoomStatusDTO>.ResponseSuccess("Room Status updated successfully.");
            }
            catch (Exception ex)
            {
				return ServiceResponse<RoomStatusDTO>.ResponseFailure(ex.Message);
			}
        }

        public async Task<ServiceResponse<object>> DeleteRoomStatusByIdAsync(int Id)
        {
            int? branchId = BranchContext.CurrentBranchId;

			var roomStatus = await _unitOfWork.Repository<RoomStatus>()
			    .GetByIdAsync(Id, rs => rs.IsSystem || rs.BranchId == branchId, SkipBranchFilter: true);
			if (roomStatus == null)
            {
                return ServiceResponse<object>.ResponseFailure("not found.");
            }

			if (roomStatus.IsSystem)
			{
				return ServiceResponse<object>.ResponseFailure("Can't delete system room status.");
			}

			try
            {
                _unitOfWork.Repository<RoomStatus>().Delete(roomStatus);
                await _unitOfWork.CommitAsync();
                return ServiceResponse<object>.ResponseSuccess("Room status deleted successfully.");
            }
            catch (Exception ex)
            {
				return ServiceResponse<object>.ResponseFailure(ex.Message);
			}
		}
	}
}

using AutoMapper;
using AutoMapper.Configuration.Annotations;
using HotelApp.Application.DTOs.Floor;
using HotelApp.Application.IRepositories;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.FloorService
{
    public class FloorService : IFloorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

		public FloorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
		}
         
        public async Task<IEnumerable<GetFloorItemsDTO>> GetFloorItemsAsync()
        {
			var floors = await _unitOfWork.Repository<Floor>().GetAllAsDtoAsync<GetFloorItemsDTO>();
            return floors;
        }
		public async Task<IEnumerable<GetAllFloorsDTO>> GetAllFloorsAsync()
        {
            var floors = await _unitOfWork.Repository<Floor>().GetAllAsDtoAsync<GetAllFloorsDTO>();
            return floors;
        }
		public async Task<GetFloorByIdDTO?> GetFloorByIdAsync(int Id)
        {
            var floor = await _unitOfWork.Repository<Floor>().GetByIdAsDtoAsync<GetFloorByIdDTO>(Id);
            return floor;
        }
		public async Task<ServiceResponse<AddFloorDTO>> AddFloorAsync(AddFloorDTO floor)
        {
            bool IsFloorNumberExist = await _unitOfWork.Repository<Floor>().IsExistsAsync(f => f.Number == floor.Number && f.BranchId == floor.BranchId);
            if (IsFloorNumberExist)
            {
                return ServiceResponse<AddFloorDTO>.ResponseFailure("Can not enter duplicate Floor number.");
            }
            try
            {
                var mappedFloor = _mapper.Map<Floor>(floor);
                await _unitOfWork.Repository<Floor>().AddNewAsync(mappedFloor);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<AddFloorDTO>.ResponseSuccess();
            }
            catch (Exception ex)
            {
                return ServiceResponse<AddFloorDTO>.ResponseFailure(ex.Message);
            }
		}
		public async Task<EditFloorDTO?> GetFloorToEditByIdAsync(int Id)
        {
			var floor = await _unitOfWork.Repository<Floor>().GetByIdAsDtoAsync<EditFloorDTO>(Id);
			return floor;
		}
		public async Task<ServiceResponse<EditFloorDTO>> EditFloorAsync(EditFloorDTO floorDTO)
        {
            var OldFloor = await _unitOfWork.Repository<Floor>().GetByIdAsync(floorDTO.Id);

            bool IsFloorNumberExist = await _unitOfWork.Repository<Floor>()
                .IsExistsAsync(f => f.Number == floorDTO.Number && f.Id != floorDTO.Id && f.BranchId == OldFloor.BranchId);
            if (IsFloorNumberExist)
            {
                return ServiceResponse<EditFloorDTO>.ResponseFailure("Can not enter duplicate Floor number.");
            }
            try
            {
                _mapper.Map(floorDTO, OldFloor);
                _unitOfWork.Repository<Floor>().Update(OldFloor);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<EditFloorDTO>.ResponseSuccess("Floor updated successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<EditFloorDTO>.ResponseFailure(ex.InnerException.Message);
            }
        }
        public async Task<ServiceResponse<Floor>> DeleteFloorAsync(int Id)
        {
            var floor = await _unitOfWork.Repository<Floor>().GetByIdAsync(Id);
            if (floor == null)
            {
                return ServiceResponse<Floor>.ResponseFailure("Floor not found.");
            }
            bool IsAnyRoomAssignedToFloor = await _unitOfWork.Repository<Room>().IsExistsAsync(r => r.FloorId == Id);
            if (IsAnyRoomAssignedToFloor)
            {
                return ServiceResponse<Floor>.ResponseFailure("Can not delete a floor that assigned to a room.");
            }
            try
            {
                _unitOfWork.Repository<Floor>().Delete(floor);
                await _unitOfWork.CommitAsync();
                return ServiceResponse<Floor>.ResponseSuccess("Floor Deleted Successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Floor>.ResponseFailure(ex.Message);
            }
        }
    }
}

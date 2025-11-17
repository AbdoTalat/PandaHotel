using AutoMapper;
using AutoMapper.Configuration.Annotations;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Floor;
using HotelApp.Application.Interfaces;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
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
         
        public async Task<IEnumerable<SelectListItem>> GetFloorsDropDownAsync()
        {
			var floors = await _unitOfWork.FloorRepository.GetAllAsDtoAsync<SelectListItem>();
            return floors;
        }
		public async Task<IEnumerable<GetAllFloorsDTO>> GetAllFloorsAsync()
        {
            var floors = await _unitOfWork.FloorRepository.GetAllAsDtoAsync<GetAllFloorsDTO>();
            return floors;
        }
		public async Task<GetFloorByIdDTO?> GetFloorByIdAsync(int Id)
        {
            var floor = await _unitOfWork.FloorRepository.GetByIdAsDtoAsync<GetFloorByIdDTO>(Id);
            return floor;
        }
		public async Task<ServiceResponse<FloorDTO>> AddFloorAsync(FloorDTO floor)
        {
            bool IsFloorNumberExist = await _unitOfWork.FloorRepository.AnyAsync(f => f.Number == floor.Number);
            if (IsFloorNumberExist)
            {
                return ServiceResponse<FloorDTO>.ResponseFailure("Can not enter duplicate Floor number.");
            }
            try
            {
                var mappedFloor = _mapper.Map<Floor>(floor);
                await _unitOfWork.FloorRepository.AddNewAsync(mappedFloor);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<FloorDTO>.ResponseSuccess();
            }
            catch (Exception ex)
            {
                return ServiceResponse<FloorDTO>.ResponseFailure(ex.Message);
            }
		}
		public async Task<FloorDTO?> GetFloorToEditByIdAsync(int Id)
        {
			var floor = await _unitOfWork.FloorRepository.GetByIdAsDtoAsync<FloorDTO>(Id);
			return floor;
		}
		public async Task<ServiceResponse<FloorDTO>> EditFloorAsync(FloorDTO floorDTO)
        {
            var OldFloor = await _unitOfWork.FloorRepository.GetByIdAsync(floorDTO.Id);

            bool IsFloorNumberExist = await _unitOfWork.FloorRepository
                .AnyAsync(f => f.Number == floorDTO.Number && f.Id != floorDTO.Id && f.BranchId == OldFloor.BranchId);
            if (IsFloorNumberExist)
            {
                return ServiceResponse<FloorDTO>.ResponseFailure("Can not enter duplicate Floor number.");
            }
            try
            {
                _mapper.Map(floorDTO, OldFloor);
                _unitOfWork.FloorRepository.Update(OldFloor);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<FloorDTO>.ResponseSuccess("Floor updated successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<FloorDTO>.ResponseFailure(ex.InnerException.Message);
            }
        }
        public async Task<ServiceResponse<Floor>> DeleteFloorAsync(int Id)
        {
            var floor = await _unitOfWork.FloorRepository.GetByIdAsync(Id);
            if (floor == null)
            {
                return ServiceResponse<Floor>.ResponseFailure("Floor not found.");
            }
            bool IsAnyRoomAssignedToFloor = await _unitOfWork.RoomRepository.AnyAsync(r => r.FloorId == Id);
            if (IsAnyRoomAssignedToFloor)
            {
                return ServiceResponse<Floor>.ResponseFailure("Can not delete a floor that assigned to a room.");
            }
            try
            {
                _unitOfWork.FloorRepository.Delete(floor);
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

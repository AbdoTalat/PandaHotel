using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Application.Interfaces;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.RoomTypeService
{
    public class RoomTypeService : IRoomTypeService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public RoomTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<GetAllRoomTypesDTO>> GetAllRoomTypesAsync()
		{
			var roomTypes = await _unitOfWork.RoomTypeRepository
				.GetAllAsDtoAsync<GetAllRoomTypesDTO>();

			return roomTypes;
		}
		public async Task<IEnumerable<DropDownDTO<string>>> GetRoomTypesDropDownAsync()
		{
			var roomTypes = await _unitOfWork.RoomTypeRepository
				.GetAllAsDtoAsync<DropDownDTO<string>>();

			return roomTypes;
		}
		public async Task<GetRoomTypeByIdDTO?> GetRoomTypeByIdAsync(int Id)
		{
			var roomType = await _unitOfWork.RoomTypeRepository.GetByIdAsDtoAsync<GetRoomTypeByIdDTO>(Id);
			return roomType;
		}
        public async Task<RoomTypeDTO?> GetRoomTypeToEditByIdAsync(int Id)
		{
			var roomType = await _unitOfWork.RoomTypeRepository.GetByIdAsDtoAsync<RoomTypeDTO>(Id);
			return roomType;
		}
        public async Task<IEnumerable<GetRoomTypesForReservationDTO>> GetRoomTypesForReservationAsync()
		{
			var roomTypes = await _unitOfWork.RoomTypeRepository.GetRoomTypesForReservation();
			
			return roomTypes;
        }
        public async Task<ServiceResponse<RoomTypeDTO>> AddRoomTypeAsync(RoomTypeDTO roomTypeDTO)
		{
			try
			{
				var isTypeNameExist = await _unitOfWork.RoomTypeRepository
					.IsExistsAsync(rt => rt.Name.ToLower() == roomTypeDTO.Name.ToLower());
				if (isTypeNameExist)
				{
					return ServiceResponse<RoomTypeDTO>.ResponseFailure("cannot enter duplicate room type name.");
				}

				var mappedRoomType = _mapper.Map<RoomType>(roomTypeDTO);
				await _unitOfWork.RoomTypeRepository.AddNewAsync(mappedRoomType);
				await _unitOfWork.CommitAsync();

				return ServiceResponse<RoomTypeDTO>.ResponseSuccess("New Roomtype added successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<RoomTypeDTO>.ResponseFailure($"Error Occurred: {ex.Message}");
			}
		}
        public async Task<ServiceResponse<RoomTypeDTO>> EditRoomTypeAsync(RoomTypeDTO roomTypeDTO)
		{
			var oldRoomType = await _unitOfWork.RoomTypeRepository.GetByIdAsync(roomTypeDTO.Id);
			if (oldRoomType == null)
			{
				return ServiceResponse<RoomTypeDTO>.ResponseFailure($"Room Type not found.");
			}
			var isTypeNameExist = await _unitOfWork.RoomTypeRepository
				.IsExistsAsync(rt => rt.Name.ToLower() == roomTypeDTO.Name.ToLower() && rt.Id != roomTypeDTO.Id);
			if (isTypeNameExist)
			{
				return ServiceResponse<RoomTypeDTO>.ResponseFailure("cannot enter duplicate room type name.");
			}
			try
			{
				_mapper.Map(roomTypeDTO, oldRoomType);

				_unitOfWork.RoomTypeRepository.Update(oldRoomType);
				await _unitOfWork.CommitAsync();


				//await _unitOfWork.RoomRepository.BulkUpdateAsync(
				//	 r => r.RoomTypeId == roomTypeDTO.Id && r.IsAffectedByRoomType,
				//	 setters => setters
				//		 .SetProperty(r => r.PricePerNight, roomTypeDTO.PricePerNight)
				//		 .SetProperty(r => r.MaxNumOfAdults, roomTypeDTO.MaxNumOfAdults)
				//		 .SetProperty(r => r.MaxNumOfChildrens, roomTypeDTO.MaxNumOfChildrens),
				//	 skipAuditFields:true
				// );



				return ServiceResponse<RoomTypeDTO>.ResponseSuccess($"{oldRoomType.Name} Room type updated successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<RoomTypeDTO>.ResponseFailure($"Error Occurred while saving: {ex.Message}");
			}
		}
		public async Task<ServiceResponse<RoomType>> DeleteRoomTypeAsync(int Id)
		{
			var roomType = await _unitOfWork.RoomTypeRepository.GetByIdAsync(Id);
			if (roomType == null)
			{
				return ServiceResponse<RoomType>.ResponseFailure($"Room Type With ID: {Id} not found");
			}
			if (roomType.IsActive)
			{
				return ServiceResponse<RoomType>.ResponseFailure("This room type cannot be deleted because it is assigned to existing rooms.");
			}
			try
			{
				var IsRoomTypeAssigned = await _unitOfWork.RoomRepository.IsExistsAsync(r => r.RoomTypeId == Id);
				if (IsRoomTypeAssigned)
				{
					return ServiceResponse<RoomType>.ResponseFailure("This Room type is in use and cannot be deleted.");
				}

				_unitOfWork.RoomTypeRepository.Delete(roomType);
				await _unitOfWork.CommitAsync();
				return ServiceResponse<RoomType>.ResponseSuccess($"Room Type {roomType.Name} Deleted Successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<RoomType>.ResponseFailure($"Error Occurred While Deleting: {ex.Message}");
			}
		}
	}
}

using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.ReservationSource;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelApp.Application.Interfaces;

namespace HotelApp.Application.Services.ReservationSourceService
{
    public class ReservationSourceService : IReservationSourceService
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ReservationSourceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
		}

        public async Task<IEnumerable<DropDownDTO<string>>> GetReservationSourcesDropDownAsync()
        {
            var reservationSources = await _unitOfWork.ReservationSourceRepository
                .GetAllAsDtoAsync<DropDownDTO<string>>(SkipBranchFilter:true);

            return reservationSources;
        }

		public async Task<IEnumerable<ReservationSourceDTO>> GetAllReservationSourcesAsync()
		{
			var ReservationSources = await _unitOfWork.ReservationSourceRepository
				.GetAllAsDtoAsync<ReservationSourceDTO>();

			return ReservationSources;
		}

		public async Task<ServiceResponse<ReservationSourceDTO>> AddReservationSourceAsync(ReservationSourceDTO dto)
		{
			try
			{
				var ReservationSource = _mapper.Map<ReservationSource>(dto);
				await _unitOfWork.ReservationSourceRepository.AddNewAsync(ReservationSource);
				await _unitOfWork.CommitAsync();

				return ServiceResponse<ReservationSourceDTO>.ResponseSuccess("New Reservation Source added successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<ReservationSourceDTO>.ResponseFailure(ex.Message);
			}
		}
		public async Task<ReservationSourceDTO?> GetReservationSourceToEditByIdAsync(int Id)
		{
			var ReservationSource = await _unitOfWork.ReservationSourceRepository
				.GetByIdAsDtoAsync<ReservationSourceDTO>(Id);

			return ReservationSource;
		}
		public async Task<ServiceResponse<ReservationSourceDTO>> EditReservationSourceAsync(ReservationSourceDTO dto)
		{
			var OldReservationSource = await _unitOfWork.ReservationSourceRepository
				.GetByIdAsync(dto.Id);
			if (OldReservationSource == null)
			{
				return ServiceResponse<ReservationSourceDTO>.ResponseFailure($"No Reservation Source with this ID {dto.Id}");
			}
			try
			{
				_mapper.Map(dto, OldReservationSource);

				_unitOfWork.ReservationSourceRepository.Update(OldReservationSource);
				await _unitOfWork.CommitAsync();

				return ServiceResponse<ReservationSourceDTO>.ResponseSuccess("Reservation Source Updated Successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<ReservationSourceDTO>.ResponseFailure(ex.Message);
			}

		}
		public async Task<ServiceResponse<object>> DeleteReservationSourceAsync(int Id)
		{
			bool IsExist = await _unitOfWork.ReservationSourceRepository
				.IsExistsAsync(pt => pt.Id == Id);
			if (!IsExist)
			{
				return ServiceResponse<object>.ResponseFailure($"No Reservation Source with this ID {Id}");
			}
			try
			{
				await _unitOfWork.ReservationSourceRepository.DeleteByIdAsync(Id);
				await _unitOfWork.CommitAsync();
				return ServiceResponse<object>.ResponseSuccess("Reservation Source Deleted Successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<object>.ResponseFailure(ex.Message);
			}
		}

	}
}

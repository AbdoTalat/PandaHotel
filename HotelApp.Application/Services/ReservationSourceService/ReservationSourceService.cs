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
            var reservationSources = await _unitOfWork.Repository<ReservationSource>()
                .GetAllAsDtoAsync<DropDownDTO<string>>(SkipBranchFilter:true);

            return reservationSources;
        }

		public async Task<IEnumerable<ReservationSourceDTO>> GetAllReservationSourcesAsync()
		{
			var ReservationSources = await _unitOfWork.Repository<ReservationSource>()
				.GetAllAsDtoAsync<ReservationSourceDTO>();

			return ReservationSources;
		}

		public async Task<ServiceResponse<ReservationSourceDTO>> AddReservationSourceAsync(ReservationSourceDTO dto)
		{
			try
			{
				var ReservationSource = _mapper.Map<ReservationSource>(dto);
				await _unitOfWork.Repository<ReservationSource>().AddNewAsync(ReservationSource);
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
			var ReservationSource = await _unitOfWork.Repository<ReservationSource>()
				.GetByIdAsDtoAsync<ReservationSourceDTO>(Id);

			return ReservationSource;
		}
		public async Task<ServiceResponse<ReservationSourceDTO>> EditReservationSourceAsync(ReservationSourceDTO dto)
		{
			var OldReservationSource = await _unitOfWork.Repository<ReservationSource>()
				.GetByIdAsync(dto.Id);
			if (OldReservationSource == null)
			{
				return ServiceResponse<ReservationSourceDTO>.ResponseFailure($"No Reservation Source with this ID {dto.Id}");
			}
			try
			{
				_mapper.Map(dto, OldReservationSource);

				_unitOfWork.Repository<ReservationSource>().Update(OldReservationSource);
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
			bool IsExist = await _unitOfWork.Repository<ReservationSource>()
				.IsExistsAsync(pt => pt.Id == Id);
			if (!IsExist)
			{
				return ServiceResponse<object>.ResponseFailure($"No Reservation Source with this ID {Id}");
			}
			try
			{
				await _unitOfWork.Repository<ReservationSource>().DeleteByIdAsync(Id);
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

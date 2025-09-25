using AutoMapper;
using AutoMapper.Configuration.Annotations;
using HotelApp.Application.DTOs.Rates;
using HotelApp.Application.DTOs.RoleBased;
using HotelApp.Application.IRepositories;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelApp.Application.Services.RateService
{
    public class RateService : IRateService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        private readonly IRateRepository _rateRepository;

        public RateService(IUnitOfWork unitOfWork, IMapper mapper, IRateRepository rateRepository)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
            _rateRepository = rateRepository;
        }

		public async Task<IEnumerable<GetAllRatesDTO>> GetAllRatesAsync()
		{
			var rates = await _unitOfWork.Repository<Rate>().GetAllAsDtoAsync<GetAllRatesDTO>();

			return rates;
		}
		public async Task<RateDTO?> GetRateToEditByIdAsync(int Id)
		{
			var rate = await _rateRepository.GetRateToEditByIdAsync(Id);

			return rate;
		}
		public async Task<IEnumerable<GetRatesForReservationResponseDTO>> GetRatesForReservationAsync(RatesForReservationRequestDTO model)
		{
			var rates = await _rateRepository.GetRatesForReservationAsync(model);

			return rates;
		}
        public async Task<IEnumerable<GetRateDetailsForReservationDTO>> GetRateDetailsForReservation(int rateId, int BranchId)
		{
			var rateDetails = await _unitOfWork.Repository<RoomTypeRate>()
				.GetAllAsDtoAsync<GetRateDetailsForReservationDTO>(rtr => rtr.RateId == rateId && rtr.RoomType.BranchId == BranchId);

			return rateDetails;
        }
       
		public async Task<ServiceResponse<RateDTO>> AddRateAsync(RateDTO rateDTO)
        {
            try
            {
				await _unitOfWork.BeginTransactionAsync();
                var rate = _mapper.Map<Rate>(rateDTO);
                await _unitOfWork.Repository<Rate>().AddNewAsync(rate);
				await _unitOfWork.CommitAsync();


				if (rateDTO.RoomTypeRates?.Any(r => r.IsSelected) == true)
				{
					var roomTypeRates = rateDTO.RoomTypeRates
						.Where(rtr => rtr.IsSelected)
						.Select(rtr => new RoomTypeRate
						{
							RateId = rate.Id,
							RoomTypeId = rtr.RoomTypeId,
							HourlyPrice = rtr.HourlyPrice,
							DailyPrice = rtr.DailyPrice,
							ExtraDailyPrice = rtr.ExtraDailyPrice,
							WeeklyPrice = rtr.WeeklyPrice,
							MonthlyPrice = rtr.MonthlyPrice
						});

					await _unitOfWork.Repository<RoomTypeRate>().AddRangeAsync(roomTypeRates);
					//await _unitOfWork.CommitAsync();
				}
				await _unitOfWork.CommitTransactionAsync();

				return ServiceResponse<RateDTO>.ResponseSuccess("Rate added successfully.");
			}
            catch (Exception ex)
            {
				await _unitOfWork.RollbackTransactionAsync();
                return ServiceResponse<RateDTO>.ResponseFailure(ex.Message);
            }
        }
		public async Task<ServiceResponse<RateDTO>> EditRateAsync(RateDTO rateDTO)
		{
			var OldRate = await _unitOfWork.Repository<Rate>().GetByIdAsync(rateDTO.Id);
			if (OldRate == null)
			{
				return ServiceResponse<RateDTO>.ResponseFailure("Rate not found.");
			}

			try
			{
				_mapper.Map(rateDTO, OldRate);
				_unitOfWork.Repository<Rate>().Update(OldRate);

				var existingRoomTypeRates = await _unitOfWork.Repository<RoomTypeRate>()
					.GetAllAsync(rtr => rtr.RateId == rateDTO.Id);

				var updatedRoomTypeRates = new List<RoomTypeRate>();
				var newRoomTypeRates = new List<RoomTypeRate>();
				var roomTypeRatesToDelete = new List<RoomTypeRate>();

				foreach (var roomTypeRateDTO in rateDTO.RoomTypeRates)
				{
					if (!roomTypeRateDTO.IsSelected)
					{
						var existing = existingRoomTypeRates.FirstOrDefault(rtr => rtr.Id == roomTypeRateDTO.Id);
						if (existing != null)
						{
							roomTypeRatesToDelete.Add(existing);
						}
						continue; 
					}

					if (roomTypeRateDTO.Id == 0)
					{
						var newRoomTypeRate = new RoomTypeRate
						{
							RateId = rateDTO.Id,
							RoomTypeId = roomTypeRateDTO.RoomTypeId,
							HourlyPrice = roomTypeRateDTO.HourlyPrice,
							DailyPrice = roomTypeRateDTO.DailyPrice,
							ExtraDailyPrice = roomTypeRateDTO.ExtraDailyPrice,
							WeeklyPrice = roomTypeRateDTO.WeeklyPrice,
							MonthlyPrice = roomTypeRateDTO.MonthlyPrice
						};

						//var newRoomTypeRate = _mapper.Map<RoomTypeRate>(roomTypeRateDTO);
						//newRoomTypeRate.RateId = rateDTO.Id;
						newRoomTypeRates.Add(newRoomTypeRate);
					}
					else
					{
						var existing = existingRoomTypeRates.FirstOrDefault(rtr => rtr.Id == roomTypeRateDTO.Id);
						if (existing != null)
						{
							_mapper.Map(roomTypeRateDTO, existing); 
							updatedRoomTypeRates.Add(existing);
						}
					}
				}

				if (newRoomTypeRates.Any())
				{
					await _unitOfWork.Repository<RoomTypeRate>().AddRangeAsync(newRoomTypeRates);
				}

				if (updatedRoomTypeRates.Any())
				{
					_unitOfWork.Repository<RoomTypeRate>().UpdateRange(updatedRoomTypeRates);
				}

				if (roomTypeRatesToDelete.Any())
				{
					_unitOfWork.Repository<RoomTypeRate>().DeleteRange(roomTypeRatesToDelete);
				}

				await _unitOfWork.CommitAsync();

				return ServiceResponse<RateDTO>.ResponseSuccess("Rate updated successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<RateDTO>.ResponseFailure(ex.Message);
			}
		}

	}
}

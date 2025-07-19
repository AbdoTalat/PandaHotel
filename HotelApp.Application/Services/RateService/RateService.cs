using AutoMapper;
using AutoMapper.Configuration.Annotations;
using HotelApp.Application.DTOs.Rates;
using HotelApp.Application.IRepositories;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public async Task<EditRateDTO?> GetRateToEditByIdAsync(int Id)
		{
			var rate = await _rateRepository.GetRateToEditByIdAsync(Id);

			//var rate = await _unitOfWork.Repository<Rate>().GetByIdAsDtoAsync<EditRateDTO>(Id);
			return rate;
		}

		public async Task<IEnumerable<GetRatesForReservationResponseDTO>> GetRatesForReservationAsync(RatesForReservationRequestDTO model)
		{
			var roomTypeCount = model.RoomTypeIds.Count();

			var rates = await _unitOfWork.Repository<RoomTypeRate>()
				.GetAllIQueryable()
				.Include(rtr => rtr.Rate)
				.Where(rtr => model.RoomTypeIds.Contains(rtr.RoomTypeId) &&
					rtr.Rate.BranchId == 2 &&
					rtr.Rate != null &&
					rtr.Rate.IsActive &&
					rtr.Rate.EffectiveDate <= model.CheckInDate &&
					rtr.Rate.EndDate >= model.CheckOutDate)
				.GroupBy(rtr => new 
				{
					rtr.Rate.Id,
					rtr.Rate.Code 
				})
				.Where(group => group.Select(rtr => rtr.RoomTypeId).Distinct().Count() == roomTypeCount)
				.Select(group => new GetRatesForReservationResponseDTO
				{
					Id = group.Key.Id,
					Name = group.Key.Code
				})
				.Distinct()
				.ToListAsync();

			return rates;
		}

        public async Task<IEnumerable<GetRateDetailsForReservationDTO>> GetRateDetailsForReservation(int rateId)
		{
            var rateDetails = await _unitOfWork.Repository<RoomTypeRate>().GetAllIQueryable()
                .Where(rtr => rtr.Rate.Id == rateId)
                .Select(rtr => new GetRateDetailsForReservationDTO
                {
                    typeName = rtr.RoomType.Name,
                    hourlyPrice = rtr.HourlyPrice,
                    dailyPrice = rtr.DailyPrice,
                    extraDailyPrice = rtr.ExtraDailyPrice,
                    weeklyPrice = rtr.WeeklyPrice,
                    monthlyPrice = rtr.MonthlyPrice
                })
                .ToListAsync();

			return rateDetails;
        }
       
		public async Task<ServiceResponse<AddRateDTO>> AddRateAsync(AddRateDTO rateDTO)
        {
            try
            {
                var rate = _mapper.Map<Rate>(rateDTO);
                await _unitOfWork.Repository<Rate>().AddNewAsync(rate);
                await _unitOfWork.CommitAsync();


                var roomTypeRate = rateDTO.RoomTypeRates.Where(rt => rt.IsSelected).Select(rt => new RoomTypeRate
                {
                    RateId = rate.Id,
                    RoomTypeId = rt.RoomTypeId,
                    HourlyPrice = rt.HourlyPrice,
                    DailyPrice = rt.DailyPrice,
                    ExtraDailyPrice = rt.ExtraDailyPrice,
                    WeeklyPrice = rt.WeeklyPrice,
                    MonthlyPrice = rt.MonthlyPrice
                });

                await _unitOfWork.Repository<RoomTypeRate>().AddRangeAsync(roomTypeRate);
                await _unitOfWork.CommitAsync();

				return ServiceResponse<AddRateDTO>.ResponseSuccess("Rate added successfully.");
			}
            catch (Exception ex)
            {
                return ServiceResponse<AddRateDTO>.ResponseFailure(ex.Message);
            }
        }

		public async Task<ServiceResponse<EditRateDTO>> EditRateAsync(EditRateDTO rateDTO)
		{
			var OldRate = await _unitOfWork.Repository<Rate>().GetByIdAsync(rateDTO.Id);
			if (OldRate == null)
			{
				return ServiceResponse<EditRateDTO>.ResponseFailure("Rate not found.");
			}

			try
			{
				// Update the Rate entity
				var rate = _mapper.Map(rateDTO, OldRate);
				_unitOfWork.Repository<Rate>().Update(OldRate);

				// Fetch existing RoomTypeRates from the database
				var existingRoomTypeRates = await _unitOfWork.Repository<RoomTypeRate>()
					.GetAllIQueryable()
					.Where(rtr => rtr.RateId == rateDTO.Id)
					.ToListAsync();

				var updatedRoomTypeRates = new List<RoomTypeRate>();
				var newRoomTypeRates = new List<RoomTypeRate>();
				var roomTypeRatesToDelete = new List<RoomTypeRate>();

				foreach (var roomTypeRateDTO in rateDTO.roomTypeRateDTOs)
				{
					if (!roomTypeRateDTO.IsSelected)
					{
						// Mark for deletion if it's in the database
						var existing = existingRoomTypeRates.FirstOrDefault(rtr => rtr.Id == roomTypeRateDTO.Id);
						if (existing != null)
						{
							roomTypeRatesToDelete.Add(existing);
						}
						continue; // Skip processing further for unselected items
					}

					if (roomTypeRateDTO.Id == 0)
					{
						// Add new RoomTypeRate if Id == 0
						var newRoomTypeRate = _mapper.Map<RoomTypeRate>(roomTypeRateDTO);
						newRoomTypeRate.RateId = rateDTO.Id;
						newRoomTypeRates.Add(newRoomTypeRate);
					}
					else
					{
						// Update existing RoomTypeRate if Id > 0
						var existing = existingRoomTypeRates.FirstOrDefault(rtr => rtr.Id == roomTypeRateDTO.Id);
						if (existing != null)
						{
							_mapper.Map(roomTypeRateDTO, existing); // Update existing entity
							updatedRoomTypeRates.Add(existing);
						}
					}
				}

				// Perform database operations
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

				return ServiceResponse<EditRateDTO>.ResponseSuccess("Rate updated successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<EditRateDTO>.ResponseFailure(ex.Message);
			}
		}

	}
}

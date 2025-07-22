using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Application.DTOs.Rates;
using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Application.IRepositories;
using HotelApp.Domain.Entities;
using HotelApp.Domain;
using HotelApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Application.DTOs.RoleBased;
using Microsoft.AspNetCore.Http;
using HotelApp.Helper;

namespace HotelApp.Infrastructure.Repositories
{
    public class RateRepository : IRateRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public RateRepository(ApplicationDbContext context,
            IConfigurationProvider mapperConfig, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapperConfig = mapperConfig;
			_httpContextAccessor = httpContextAccessor;
		}

        public async Task<EditRateDTO> GetRateToEditByIdAsync(int Id)
        {
            var rateDto = await _context.Rates
                .Where(r => r.Id == Id)
                .Select(r => new EditRateDTO
                {
                    Id = r.Id,
                    Code = r.Code,
                    EffectiveDate = r.EffectiveDate,
                    EndDate = r.EndDate,
                    MinChargeDayes = r.MinChargeDayes,
                    IsActive = r.IsActive,
                    SkipHourly = r.SkipHourly,
                    DisplayOnline = r.DisplayOnline
                })
                .FirstOrDefaultAsync();

            if (rateDto == null)
                return null;

            var roomTypeRates = await _context.RoomTypeRates
                .Where(rtr => rtr.RateId == Id)
                .Select(rtr => new
                {
                    rtr.Id,
                    rtr.RateId,
                    rtr.RoomTypeId,
                    rtr.HourlyPrice,
                    rtr.DailyPrice,
                    rtr.ExtraDailyPrice,
                    rtr.WeeklyPrice,
                    rtr.MonthlyPrice
                })
                .ToListAsync();

            var roomTypes = await _context.RoomTypes
                .Select(rt => new { rt.Id, rt.Name })
                .ToListAsync();

            rateDto.roomTypeRateDTOs = roomTypes.Select(rt =>
            {
                var matchedRate = roomTypeRates.FirstOrDefault(rtr => rtr.RoomTypeId == rt.Id);

                return new EditRoomTypeRateDTO
                {
                    Id = matchedRate?.Id ?? 0,
                    RateId = Id,
                    RoomTypeId = rt.Id,
                    RoomTypeName = rt.Name,
                    HourlyPrice = matchedRate?.HourlyPrice ?? 0,
                    DailyPrice = matchedRate?.DailyPrice ?? 0,
                    ExtraDailyPrice = matchedRate?.ExtraDailyPrice ?? 0,
                    WeeklyPrice = matchedRate?.WeeklyPrice ?? 0,
                    MonthlyPrice = matchedRate?.MonthlyPrice ?? 0,
                    IsSelected = matchedRate != null
                };
            }).ToList();

            return rateDto;
        }

		public async Task<IEnumerable<GetRatesForReservationResponseDTO>> GetRatesForReservationAsync(RatesForReservationRequestDTO model)
		{
			var roomTypeCount = model.RoomTypeIds.Count();


			var rates = await _context.Rates
	            .BranchFilter() 
	            .Where(rate =>
		            rate.IsActive &&
		            rate.EffectiveDate <= model.CheckInDate &&
		            rate.EndDate >= model.CheckOutDate)
	            .Select(rate => new
	            {
		            Rate = rate,
		            RoomTypeRates = rate.RoomTypeRates
			            .Where(rtr => model.RoomTypeIds.Contains(rtr.RoomTypeId))
	            })
	            .Where(x => x.RoomTypeRates
		            .Select(rtr => rtr.RoomTypeId)
		            .Distinct()
		            .Count() == roomTypeCount)
	            .Select(x => new GetRatesForReservationResponseDTO
	            {
		            Id = x.Rate.Id,
		            Name = x.Rate.Code
	            })
	            .Distinct()
	            .ToListAsync();


			return rates;
		}
	}
}

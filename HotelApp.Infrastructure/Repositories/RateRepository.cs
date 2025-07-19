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

            int? branchId = _httpContextAccessor.HttpContext.Session.GetInt32("DefaultBranchId");

			var rates = await _context.RoomTypeRates
				.Where(rtr => model.RoomTypeIds.Contains(rtr.RoomTypeId) &&
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
	}
}

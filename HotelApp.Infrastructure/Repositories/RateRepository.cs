using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Application.DTOs.Rates;
using HotelApp.Application.DTOs.RoomTypes;
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
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Application.Services.CurrentUserService;

namespace HotelApp.Infrastructure.Repositories
{
    public class RateRepository : GenericRepository<Rate>, IRateRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;

		public RateRepository(ApplicationDbContext context, IConfigurationProvider mapperConfig)
             : base(context, mapperConfig)
        {
            _context = context;
            _mapperConfig = mapperConfig;
		}

        public async Task<RateDTO> GetRateToEditByIdAsync(int Id)
        {
            var rateDto = await _context.Rates
                .BranchFilter()
                .Where(r => r.Id == Id)
                .Select(r => new RateDTO
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
                .BranchFilter()
                .Select(rt => new { rt.Id, rt.Name })
                .ToListAsync();

            rateDto.RoomTypeRates = roomTypes.Select(rt =>
            {
                var matchedRate = roomTypeRates.FirstOrDefault(rtr => rtr.RoomTypeId == rt.Id);

                return new RoomTypeRateDTO
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
			int requiredRoomTypeCount = model.RoomTypeIds.Count;

			var rates = await _context.Rates
				.AsNoTracking()
				.BranchFilter()
				.Where(rate =>
					rate.IsActive &&
					rate.EffectiveDate <= model.CheckInDate &&
					rate.EndDate >= model.CheckOutDate)
				.Select(rate => new
				{
					RateId = rate.Id,
					RateCode = rate.Code,
					RoomTypeRates = rate.RoomTypeRates
						.Where(rtr => model.RoomTypeIds.Contains(rtr.RoomTypeId))
						.Select(rtr => new
						{
							rtr.RoomTypeId,
							rtr.DailyPrice,
							RoomTypeName = rtr.RoomType.Name
						})
				})
				.Where(x =>
					x.RoomTypeRates
						.Select(r => r.RoomTypeId)
						.Distinct()
						.Count() == requiredRoomTypeCount)
				.Select(x => new GetRatesForReservationResponseDTO
				{
					Id = x.RateId,
					Name = x.RateCode,
					roomTypeRateForReservations = x.RoomTypeRates
						.Select(rtr => new RoomTypeRateForReservationDTO
						{
							TypeName = rtr.RoomTypeName,
							DailyPrice = rtr.DailyPrice
						})
						.ToList()
				})
				.ToListAsync();

			return rates;
		}
	}
}

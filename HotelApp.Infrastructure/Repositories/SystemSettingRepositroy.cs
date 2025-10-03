using AutoMapper;
using HotelApp.Application.DTOs.SystemSetting;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Repositories
{
    public class SystemSettingRepositroy : GenericRepository<SystemSetting>, ISystemSettingRepositroy
	{
		private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;

        public SystemSettingRepositroy(ApplicationDbContext context, IConfigurationProvider mapperConfig)
			: base(context, mapperConfig)
		{
			_context = context;
            _mapperConfig = mapperConfig;
        }

		public GetSystemSettingForValidationDTO? GetSystemSettingForValidation()
		{
			var result = _context.SystemSettings
				.Select(st => new GetSystemSettingForValidationDTO
				{
					IsGuestEmailRequired = st.IsGuestEmailRequired,
					IsGuestAddressRequired = st.IsGuestAddressRequired,
					IsGuestDateOfBirthRequired = st.IsGuestDateOfBirthRequired,
					IsGuestPhoneRequired = st.IsGuestPhoneRequired,
					IsGuestProofNumberRequired = st.IsGuestProofNumberRequired,
					IsGuestProofTypeRequired = st.IsGuestProofTypeRequired,
				})
				.FirstOrDefault();

			return result;
		}
	}
}

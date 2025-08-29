using HotelApp.Application.DTOs.SystemSetting;
using HotelApp.Application.IRepositories;
using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Repositories
{
	public class SystemSettingRepositroy : ISystemSettingRepositroy
	{
		private readonly ApplicationDbContext _context;

		public SystemSettingRepositroy(ApplicationDbContext context)
		{
			_context = context;
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

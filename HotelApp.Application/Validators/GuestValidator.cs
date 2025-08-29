using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.Services.SystemSettingService;

namespace HotelApp.Application.Validators
{
	public class GuestValidator : AbstractValidator<GuestDTO>
	{
		public GuestValidator(ISystemSettingService settingsService)
		{
			var settings = settingsService.GetSystemSettingForValidation();

			if (settings.IsGuestEmailRequired)
			{
				RuleFor(x => x.Email)
					.NotEmpty()
					.WithMessage("This field is required");
			}
			if (settings.IsGuestAddressRequired)
			{
				RuleFor(x => x.Address)
					.NotEmpty()
					.WithMessage("This field is required");
			}
			if (settings.IsGuestPhoneRequired)
			{
				RuleFor(x => x.Phone)
					.NotEmpty()
					.WithMessage("This field is required");
			}
			if (settings.IsGuestDateOfBirthRequired)
			{
				RuleFor(x => x.DateOfBirth)
					.NotEmpty()
					.WithMessage("This field is required");
			}
			if (settings.IsGuestProofNumberRequired)
			{
				RuleFor(x => x.ProofNumber)
					.NotEmpty()
					.WithMessage("This field is required");
			}
			if (settings.IsGuestProofTypeRequired)
			{
				RuleFor(x => x.ProofTypeId)
					.NotEmpty()
					.WithMessage("This field is required");
			}
		}
	}

}

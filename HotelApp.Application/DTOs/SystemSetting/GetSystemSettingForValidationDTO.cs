using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.SystemSetting
{
	public class GetSystemSettingForValidationDTO
	{
		public bool IsGuestEmailRequired { get; set; }
        public bool IsGuestDateOfBirthRequired { get; set; }
        public bool IsGuestPhoneRequired { get; set; }
        public bool IsGuestAddressRequired { get; set; }
        public bool IsGuestProofTypeRequired { get; set; }
        public bool IsGuestProofNumberRequired { get; set; }
    }
}

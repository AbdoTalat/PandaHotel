using HotelApp.Application.DTOs.SystemSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.SystemSettingService
{
	public interface ISystemSettingService
	{
		Task<SystemSettingDTO?> GetSystemSettingForEditAsync();
		Task<ServiceResponse<SystemSettingDTO>> EditSystemSettingAsync(SystemSettingDTO systemSettingDTO);
	}
}

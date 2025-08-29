using AutoMapper;
using HotelApp.Application.DTOs.SystemSetting;
using HotelApp.Application.IRepositories;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.SystemSettingService
{
	public class SystemSettingService : ISystemSettingService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ISystemSettingRepositroy _systemSettingRepositroy;

		public SystemSettingService(IUnitOfWork unitOfWork, IMapper mapper, ISystemSettingRepositroy systemSettingRepositroy)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_systemSettingRepositroy = systemSettingRepositroy;
		}
        public async Task<SystemSettingDTO?> GetSystemSettingForEditAsync()
		{
			var systemSetting = await _unitOfWork.Repository<SystemSetting>()
				.FirstOrDefaultAsDtoAsync<SystemSettingDTO>();

			return systemSetting;
		}
        public async Task<ServiceResponse<SystemSettingDTO>> EditSystemSettingAsync(SystemSettingDTO systemSettingDTO)
		{
			var OldSystemSetting = await _unitOfWork.Repository<SystemSetting>()
				.FirstOrDefaultAsync(st => st.Id == systemSettingDTO.Id);
			if (OldSystemSetting == null)
			{
				return ServiceResponse<SystemSettingDTO>.ResponseFailure("System setting is no longer exists.");
			}
			try
			{
				_mapper.Map(systemSettingDTO, OldSystemSetting);
				_unitOfWork.Repository<SystemSetting>().Update(OldSystemSetting);
				await _unitOfWork.CommitAsync();

                return ServiceResponse<SystemSettingDTO>.ResponseSuccess("New system settings saved successfuly.");
            }
            catch (Exception ex)
			{
                return ServiceResponse<SystemSettingDTO>.ResponseFailure(ex.Message);
            }
        }
		public GetSystemSettingForValidationDTO? GetSystemSettingForValidation()
		{
			var result = _systemSettingRepositroy.GetSystemSettingForValidation();

			return result;
		}
	}
}

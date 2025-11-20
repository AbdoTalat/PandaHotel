using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.SystemSetting;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Mapping
{
	public class SystemSettingMapping : Profile
	{
        public SystemSettingMapping()
        {
            CreateMap<SystemSetting, SystemSettingDTO>();
            CreateMap<SystemSettingDTO, SystemSetting>();
        }
    }
}

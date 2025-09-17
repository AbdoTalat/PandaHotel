using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Options;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Mapping
{
    public class OptionMapping : Profile 
	{
        public OptionMapping()
        {
            CreateMap<Option, GetAllOptionsDTO>();
            CreateMap<Option, DropDownDTO<string>>()
				.ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));

            CreateMap<OptionDTO, Option>();
            CreateMap<Option, OptionDTO>();
        }
    }
}

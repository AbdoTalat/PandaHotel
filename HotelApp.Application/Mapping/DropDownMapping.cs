using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Mapping
{
    public class DropDownMapping : Profile
    {
        public DropDownMapping()
        {
            CreateMap<Country, DropDownDTO<string>>()
                .ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));

            CreateMap<State, DropDownDTO<string>>()
                .ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));
        }
    }
}

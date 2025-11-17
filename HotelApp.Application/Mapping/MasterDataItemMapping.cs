using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Mapping
{
    public class MasterDataItemMapping : Profile
    {
        public MasterDataItemMapping()
        {
            CreateMap<Country, DropDownDTO<string>>()
                .ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));

            CreateMap<State, DropDownDTO<string>>()
                .ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));


            CreateMap<MasterDataItem, SelectListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
        }
    }
}

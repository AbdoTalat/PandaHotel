using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Floor;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Mapping
{
    public class FloorMapping : Profile
	{
        public FloorMapping()
        {
            CreateMap<Floor, GetAllFloorsDTO>();
            CreateMap<Floor, GetFloorByIdDTO>();

            CreateMap<Floor, FloorDTO>();
			CreateMap<FloorDTO, Floor>();

			CreateMap<FloorDTO, Floor>();

            CreateMap<Floor, SelectListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
        }
    }
}

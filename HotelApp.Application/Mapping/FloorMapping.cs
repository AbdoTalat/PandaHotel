using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Floor;
using HotelApp.Domain.Entities;
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

            CreateMap<Floor, EditFloorDTO>();
			CreateMap<EditFloorDTO, Floor>();

			CreateMap<AddFloorDTO, Floor>();

            CreateMap<Floor, DropDownDTO<int>>()
                .ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Number));
		}
    }
}

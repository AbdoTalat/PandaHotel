using AutoMapper;
using HotelApp.Application.DTOs.Rates;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Mapping
{
    public class RateMapping : Profile
    {
        public RateMapping()
        {
            CreateMap<Rate, GetAllRatesDTO>();
            CreateMap<AddRateDTO, Rate>()
                .ForMember(dest => dest.RoomTypeRates, opt => opt.Ignore());

            CreateMap<Rate, EditRateDTO>()
            .ForMember(dest => dest.roomTypeRateDTOs, opt => opt.Ignore());

            CreateMap<EditRateDTO, Rate>();


            CreateMap<EditRoomTypeRateDTO, RoomTypeRate>();

            CreateMap<RoomTypeRate, GetRateDetailsForReservationDTO>()
                .ForMember(dest => dest.typeName, opt => opt.MapFrom(src => src.RoomType.Name ?? "N/A"));
		
        }
    }
}

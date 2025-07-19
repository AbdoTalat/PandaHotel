using AutoMapper;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Mapping
{
    public class GuestMapping : Profile
	{
        public GuestMapping()
        {
			CreateMap<Guest, GetGuestByIdDTO>();
			CreateMap<Guest, GetAllGuestsDTO>();

			CreateMap<Guest, GetSearchedGuestsDTO>();

			CreateMap<AddGuestDTO, Guest>();

			CreateMap<EditGuestDTO, Guest>();
			CreateMap<Guest, EditGuestDTO>();
		}
	}
}

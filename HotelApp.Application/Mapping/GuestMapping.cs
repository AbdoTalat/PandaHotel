using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.DTOs.ProofType;
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
			CreateMap<Guest, GetGuestByIdDTO>()
				.ForMember(dest => dest.ProofTypeName, opt => opt.MapFrom(src => src.ProofType.Name));

			CreateMap<Guest, GetAllGuestsDTO>();

			CreateMap<Guest, GetSearchedGuestsDTO>();

			CreateMap<AddGuestDTO, Guest>();

			CreateMap<EditGuestDTO, Guest>();
			CreateMap<Guest, EditGuestDTO>();

			CreateMap<Guest, ReservationGuestDTO>()
				.ForMember(dest => dest.GuestId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.IsPrimary, opt => opt.Ignore());


			#region ProofType
			CreateMap<ProofType, DropDownDTO<string>>()
				.ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));

			CreateMap<ProofType, GetAllProofTypesDTO>();
            #endregion
        }
    }
}

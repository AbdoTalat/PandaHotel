using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Rooms;
using HotelApp.Application.DTOs.RoomStatus;
using HotelApp.Application.DTOs.RoomTypes;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Mapping
{
    public class RoomMapping : Profile
	{
		public RoomMapping()
		{
			/* Rooms */
			CreateMap<Room, GetAllRoomsDTO>()
		   .ForMember(dest => dest.RoomStatusName, opt => opt.MapFrom(src => src.RoomStatus.Name))
           .ForMember(dest => dest.RoomStatusColor, opt => opt.MapFrom(src => src.RoomStatus.Color))
           .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor.Number))
		   .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.RoomType.Name))
		   .ForMember(dest => dest.MaxNumOfAdults, opt => opt.MapFrom(src => src.MaxNumOfAdults))
		   .ForMember(dest => dest.MaxNumOfChildren, opt => opt.MapFrom(src => src.MaxNumOfChildrens)); // note spelling!

			CreateMap<AddRoomDTO, Room>();

            CreateMap<Room, GetRoomByIdDTO>()
				.ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.RoomType.Name))
				.ForMember(dest => dest.MaxNumOfChildrens, opt => opt.MapFrom(src => src.RoomType.MaxNumOfChildrens))
				.ForMember(dest => dest.MaxNumOfAdults, opt => opt.MapFrom(src => src.RoomType.MaxNumOfAdults))
				.ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.RoomStatus.Name))
				.ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor.Number));

            CreateMap<EditRoomDTO, Room>();

			CreateMap<Room, EditRoomDTO>()
			.ForMember(dest => dest.AssignOptions,
				opt => opt.MapFrom(src => src.RoomOptions.Select(ro => ro.OptionId)));

			CreateMap<Room, RoomsDetailsDTO>();

			CreateMap<Room, GetAvailableRoomsDTO>()
				.ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.Name));


			/* Room Types */
			CreateMap<RoomType, GetAllRoomTypesDTO>();
			CreateMap<RoomType, GetRoomTypesForReservationDTO>();
			CreateMap<RoomType, GetRoomTypeByIdDTO>();

			CreateMap<RoomType, EditRoomTypeDTO>();
			CreateMap<EditRoomTypeDTO, RoomType>();
			CreateMap<AddRoomTypeDTO, RoomType>();

			CreateMap<RoomType, DropDownDTO<string>>()
				.ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));


			/* Room Status */
			CreateMap<RoomStatus, GetAllRoomStatusDTO>();
			CreateMap<RoomStatus, DropDownDTO<string>>()
				.ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));

			CreateMap<AddRoomStatusDTO, RoomStatus>();
			CreateMap<EditRoomStatusDTO, RoomStatus>();
			CreateMap<RoomStatus, EditRoomStatusDTO>();
			
		}
	}
}

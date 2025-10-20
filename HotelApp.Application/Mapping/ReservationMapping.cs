using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.DTOs.ReservationSource;
using HotelApp.Domain.Entities;
using HotelApp.Domain.Enums;
using HotelApp.Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Mapping
{
    public class ReservationMapping : Profile
	{
        public ReservationMapping()
        {
			CreateMap<ReservationDTO, Reservation>()
			   .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.ReservationInfoDto.CheckInDate))
			   .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.ReservationInfoDto.CheckOutDate))
			   .ForMember(dest => dest.NumberOfPeople, opt => opt.MapFrom(src =>
					src.ReservationInfoDto.RoomTypeToBookDTOs.Sum(r => r.NumOfAdults + r.NumOfChildrens)))
			   .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.ReservationInfoDto.CompanyId))
			   .ForMember(dest => dest.NumberOfNights, opt => opt.MapFrom(src => src.ReservationInfoDto.NumOfNights))
			   .ForMember(dest => dest.ReservationSourceId, opt => opt.MapFrom(src => src.ReservationInfoDto.ReservationSourceId))
			   .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.ConfirmDto.Comment))
			   .ForMember(dest => dest.CancellationReason, opt => opt.MapFrom(src => src.ConfirmDto.CancellationReason))
			   .ForMember(dest => dest.Status, opt => opt.Ignore());

			CreateMap<RoomTypeToBookDTO, ReservationRoomType>()
				.ForMember(dest => dest.RoomTypeId, opt => opt.MapFrom(src => src.RoomTypeId))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.NumOfRooms))
				.ForMember(dest => dest.ReservationId, opt => opt.Ignore());


            CreateMap<Reservation, GetAllReservationsDTO>()
                .ForMember(dest => dest.PrimaryGuestName, opt => opt.MapFrom(src =>
                    src.guestReservations
                        .Where(gr => gr.IsPrimaryGuest)
                        .Select(gr => gr.Guest.FullName)
                        .FirstOrDefault()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.UserName))
                .ForMember(dest => dest.ReservationSource, opt => opt.MapFrom(src => src.ReservationSource.Name));


			CreateMap<Reservation, GetReservationDetailsByIdDTO>()
				.ForMember(dest => dest.ReservationSource, opt => opt.MapFrom(src => src.ReservationSource.Name))
				.ForMember(dest => dest.NumOfTotalRooms, opt => opt.MapFrom(src => src.ReservationsRooms.Count()));

            CreateMap<GuestReservation, GuestsDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Guest.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Guest.FullName ?? "N/A"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Guest.Email ?? "N/A"))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Guest.Phone ?? "N/A"))
                .ForMember(dest => dest.IsPrimaryGuest, opt => opt.MapFrom(src => src.IsPrimaryGuest));


            CreateMap<ReservationHistory, ReservationHistoryDTO>()
				.ForMember(dest => dest.PerformedByName, opt => opt.MapFrom(src => src.PerformedBy.UserName))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
			CreateMap<ReservationRoomType, ReservationRoomTypeDTO>()
				.ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.Name));


            /* Reservation Source */
            CreateMap<ReservationSource, DropDownDTO<string>>()
				.ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));

			CreateMap<ReservationSourceDTO, ReservationSource>();
			CreateMap<ReservationSource, ReservationSourceDTO>();


		}
	}
}

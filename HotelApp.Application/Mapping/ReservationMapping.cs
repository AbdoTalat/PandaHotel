using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.DTOs.ReservationSource;
using HotelApp.Domain.Entities;
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
			   .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.bookRoomDTO.CheckInDate))
			   .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.bookRoomDTO.CheckOutDate))
			   .ForMember(dest => dest.NumberOfPeople, opt => opt.MapFrom(src =>
					src.bookRoomDTO.roomTypeToBookDTOs.Sum(r => r.NumOfAdults + r.NumOfChildren)
				))               
			   .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.bookRoomDTO.CompanyId))
			   .ForMember(dest => dest.NumberOfNights, opt => opt.MapFrom(src => src.bookRoomDTO.NumOfNights))
               //.ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.bookRoomDTO.BranchId))
               .ForMember(dest => dest.ReservationSourceId, opt => opt.MapFrom(src => src.bookRoomDTO.ReservationSourceId))
			   .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.confirmDTO.Comment))
			   .ForMember(dest => dest.IsConfirmed, opt => opt.MapFrom(src => src.confirmDTO.IsConfirmed))
			   .ForMember(dest => dest.IsPending, opt => opt.MapFrom(src => src.confirmDTO.IsPending))
			   .ForMember(dest => dest.IsCheckedIn, opt => opt.MapFrom(src => src.confirmDTO.IsCheckedIn))
			   .ForMember(dest => dest.IsCheckedOut, opt => opt.MapFrom(src => src.confirmDTO.IsCheckedOut))
			   .ForMember(dest => dest.IsClosed, opt => opt.MapFrom(src => src.confirmDTO.IsClosed))
			   .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.confirmDTO.IsCancelled))
			   .ForMember(dest => dest.CancellationReason, opt => opt.MapFrom(src => src.confirmDTO.CancellationReason));

			CreateMap<RoomTypeToBookDTO, ReservationRoomType>()
				.ForMember(dest => dest.RoomTypeId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.NumOfRooms))
				.ForMember(dest => dest.ReservationId, opt => opt.Ignore()); 





			CreateMap<Reservation, GetAllReservationsDTO>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
					src.IsCancelled ? "Cancelled" :
					src.IsClosed ? "Closed" :
					src.IsCheckedOut ? "Checked Out" :
					src.IsCheckedIn ? "Checked In" :
					src.IsPending ? "Pending" :
					src.IsConfirmed ? "Confirmed" :
					""
				))
				.ForMember(dest => dest.PrimaryGuestName, opt => opt.MapFrom(src => src.guestReservations
                    .Where(gr => gr.IsPrimaryGuest)
                    .Select(gr => gr.Guest.FullName)
                    .FirstOrDefault()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(dst => dst.CreatedBy.UserName))
                .ForMember(dest => dest.ReservationSource, opt => opt.MapFrom(dst => dst.ReservationSource.Name));




            /* Reservation Source */
            CreateMap<ReservationSource, DropDownDTO<string>>()
				.ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));

			CreateMap<ReservationSourceDTO, ReservationSource>();
			CreateMap<ReservationSource, ReservationSourceDTO>();


		}
	}
}

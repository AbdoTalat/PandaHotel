using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.DTOs.Reservation;
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
			CreateMap<AddReservationDTO, Reservation>()
			   .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.bookRoomDTO.CheckInDate))
			   .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.bookRoomDTO.CheckOutDate))
			   .ForMember(dest => dest.NumberOfPeople, opt => opt.MapFrom(src =>
					src.bookRoomDTO.roomTypeToBookDTOs.Sum(r => r.NumOfAdults + r.NumOfChildren)
				))
			   .ForMember(dest => dest.NumberOfNights, opt => opt.MapFrom(src => src.bookRoomDTO.NumOfNights))
			   .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.bookRoomDTO.BranchId))
			   .ForMember(dest => dest.ReservationSourceId, opt => opt.MapFrom(src => src.bookRoomDTO.ReservationSourceId))
			   .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.confirmReservationDTO.Comment))
			   .ForMember(dest => dest.IsConfirmed, opt => opt.MapFrom(src => src.confirmReservationDTO.IsConfirmed))
			   .ForMember(dest => dest.IsPending, opt => opt.MapFrom(src => src.confirmReservationDTO.IsPending))
			   .ForMember(dest => dest.IsStarted, opt => opt.MapFrom(src => src.confirmReservationDTO.IsStarted))
			   .ForMember(dest => dest.IsCheckedIn, opt => opt.MapFrom(src => src.confirmReservationDTO.IsCheckedIn))
			   .ForMember(dest => dest.IsCheckedOut, opt => opt.MapFrom(src => src.confirmReservationDTO.IsCheckedOut))
			   .ForMember(dest => dest.IsClosed, opt => opt.MapFrom(src => src.confirmReservationDTO.IsClosed))
			   .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.confirmReservationDTO.IsCancelled))
			   .ForMember(dest => dest.CancellationReason, opt => opt.MapFrom(src => src.confirmReservationDTO.CancellationReason));

			CreateMap<RoomTypeToBookDTO, ReservationRoomType>()
				.ForMember(dest => dest.RoomTypeId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.NumOfRooms))
				.ForMember(dest => dest.ReservationId, opt => opt.Ignore()); // set later





			CreateMap<Reservation, GetAllReservationsDTO>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
					src.IsCancelled ? "Cancelled" :
					src.IsClosed ? "Closed" :
					src.IsCheckedOut ? "Checked Out" :
					src.IsCheckedIn ? "Checked In" :
					src.IsStarted ? "Started" :
					src.IsPending ? "Pending" :
					src.IsConfirmed ? "Confirmed" :
					""
				))
				.ForMember(dest => dest.PrimaryGuestName, opt => opt.MapFrom(src => src.guestReservations
                    .Where(gr => gr.IsPrimaryGuest)
                    .Select(gr => gr.Guest.FullName)
                    .FirstOrDefault()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(dst => dst.CreatedBy.UserName));




			/* Reservation Source */
			CreateMap<ReservationSource, DropDownDTO<string>>()
				.ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));


		}
	}
}

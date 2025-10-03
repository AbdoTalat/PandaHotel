using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.GuestService
{
    public interface IGuestService
	{
		Task<IEnumerable<GetAllGuestsDTO>> GetAllGuestsAsync();
		Task<GetGuestByIdDTO?> GetGuestByIdAsync(int Id);
		Task<GetGuestByIdDTO> GetGuestByIdWithoutBranchFilterAsync(int Id);
		Task<GuestDTO?> GetGuestToEditByIdAsync(int Id);
        Task<ServiceResponse<GuestDTO>> AddGuestAsync(GuestDTO guestDTO);
		Task<ServiceResponse<GuestDTO>> EditGuestAsync(GuestDTO guestDTO);
		Task<ServiceResponse<Guest>> DeleteGuestAsync(int Id);
		Task<IEnumerable<GetSearchedGuestsDTO>> SearchGuestsAsync(string term);
		Task<ServiceResponse<ReservationGuestDTO>> AddOrEditGuestsAsync(GuestDTO guestDTO);
	}
}

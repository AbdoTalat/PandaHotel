using HotelApp.Application.DTOs.Guests;
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
		Task<EditGuestDTO?> GetGuestToEditByIdAsync(int Id);
        Task<ServiceResponse<AddGuestDTO>> AddGuestAsync(AddGuestDTO guestDTO);
		Task<ServiceResponse<EditGuestDTO>> EditGuestAsync(EditGuestDTO guestDTO, int Id);
		Task<ServiceResponse<Guest>> DeleteGuestAsync(int Id);
		Task<IEnumerable<GetSearchedGuestsDTO>> SerachGuestsByEmailAsync(string email);
	}
}

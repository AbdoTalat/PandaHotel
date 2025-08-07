using HotelApp.Application.DTOs.Guests;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.IRepositories
{
    public interface IGuestRepository
    {
		Task<IEnumerable<GetSearchedGuestsDTO>> SerachGuestsByEmailAsync(string email);
        Task<List<GetSearchedGuestsDTO>> SearchGuestsAsync(string term);

    }
}

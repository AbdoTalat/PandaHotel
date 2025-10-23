using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Interfaces.IRepositories
{
    public interface IGuestRepository : IGenericRepository<Guest>
    {
        Task<IEnumerable<GetSearchedGuestsDTO>> SerachGuestsByEmailAsync(string email);
        Task<List<GetSearchedGuestsDTO>> SearchGuestsAsync(string term);
	}
}

using HotelApp.Application.DTOs;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.DropdownService
{
    public interface IDropdownService
    {
        Task<IEnumerable<DropDownDTO<string>>> GetAllCountriesDropDownAsync();
        Task<IEnumerable<DropDownDTO<string>>> GetAllStatesByCountryIdDropDownAsync(int countryId);
    }

}

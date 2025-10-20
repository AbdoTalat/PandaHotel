using HotelApp.Application.DTOs;
using HotelApp.Application.Interfaces;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.DropdownService
{
    public class DropdownService : IDropdownService
    {

        private readonly IUnitOfWork _unitOfWork;

        public DropdownService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<DropDownDTO<string>>> GetAllCountriesDropDownAsync()
        {
            return await _unitOfWork.CountryRepository.GetAllAsDtoAsync<DropDownDTO<string>>();
        }

        public async Task<IEnumerable<DropDownDTO<string>>> GetAllStatesByCountryIdDropDownAsync(int countryId)
        {
            return await _unitOfWork.StateRepository.GetAllAsDtoAsync<DropDownDTO<string>>(s => s.CountryId == countryId);
        }
    }
}

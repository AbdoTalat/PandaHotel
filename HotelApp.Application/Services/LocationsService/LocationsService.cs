using HotelApp.Application.Interfaces;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.LocationsService
{
    public class LocationsService : ILocationsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _unitOfWork.CountryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<State>> GetAllStatesByCountryIdAsync(int countryId)
        {
            return await _unitOfWork.StateRepository.GetAllAsync(s => s.CountryId == countryId);
        }
    }
}

using HotelApp.Application.DTOs;
using HotelApp.Application.Interfaces;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.CalculationTypeService
{
    public class CalculationTypeService : ICalculationTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CalculationTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<DropDownDTO<string>>> GetAllCalculationTypesDropDown()
        {
            var CalculationTypes = await _unitOfWork.CalculationTypeRepository
                .GetAllAsDtoAsync<DropDownDTO<string>>();

            return CalculationTypes;
        }
    }
}

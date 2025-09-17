using HotelApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.CalculationTypeService
{
    public interface ICalculationTypeService
    {
        Task<IEnumerable<DropDownDTO<string>>> GetAllCalculationTypesDropDown();
    }
}

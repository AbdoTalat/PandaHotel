using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Options;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.OptionService
{
    public interface IOptionService
	{
		Task<IEnumerable<DropDownDTO<string>>> GetOptionsDropDownAsync();
		Task<IEnumerable<GetAllOptionsDTO>> GetAllOptionsAsync();
		Task<OptionDTO?> GetOptionToEditByIdAsync(int Id);
		Task<ServiceResponse<OptionDTO>> AddOptionAsync(OptionDTO optionDTO);
		Task<ServiceResponse<OptionDTO>> EditOptionAsync(OptionDTO optionDTO);
		Task<ServiceResponse<Option>> DeleteOptionAsync(int Id);
	}
}

using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.CompanyService
{
	public interface ICompanyService
	{
		Task<IEnumerable<GetAllCompaniesDTO>> GetAllCompaniesAsync();
		Task<IEnumerable<DropDownDTO<string>>> GetCompaniesDropDownAsync();
		Task<EditCompanyDTO?> GetCompanyToEditByIdAsync(int Id);
		Task<ServiceResponse<AddCompanyDTO>> AddCompanyAsync(AddCompanyDTO companyDTO);
		Task<ServiceResponse<EditCompanyDTO>> EditCompanyAsync(EditCompanyDTO companyDTO);
		//Task<ServiceResponse<object>> DeleteCompanyByIdAsync(int Id);

		Task<IEnumerable<DropDownDTO<string>>> SerachCompanyByNameAsync(string Name);
		Task<GetSearchedCompanyDTO?> GetSearchedCompanyDataAsync(int Id);
		Task<ServiceResponse<ReservationCompanyDTO>> CreateOrUpdateCompanyAsync(ReservationCompanyDTO companyDTO);

    }
}

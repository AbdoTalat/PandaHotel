using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Company;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.CompanyService
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
		}

        public async Task<IEnumerable<GetAllCompaniesDTO>> GetAllCompaniesAsync()
        {
            var companies = await _unitOfWork.Repository<Company>()
                .GetAllAsDtoAsync<GetAllCompaniesDTO>();

            return companies;
        }

        public async Task<IEnumerable<DropDownDTO<string>>> GetCompaniesDropDownAsync()
        {
            var companies = await _unitOfWork.Repository<Company>()
                .GetAllAsDtoAsync<DropDownDTO<string>>();

            return companies;
        }

		public async Task<ServiceResponse<CompanyDTO>> AddCompanyAsync(CompanyDTO companyDTO)
        {
            try
            {
                var company = _mapper.Map<Company>(companyDTO);

                await _unitOfWork.Repository<Company>().AddNewAsync(company);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<CompanyDTO>.ResponseSuccess("new company added successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<CompanyDTO>.ResponseFailure(ex.InnerException.Message);
            }
        }

        public async Task<CompanyDTO?> GetCompanyToEditByIdAsync(int Id)
        {
            var company = await _unitOfWork.Repository<Company>()
                .GetByIdAsDtoAsync<CompanyDTO>(Id);

            return company;
        }
        public async Task<ServiceResponse<CompanyDTO>> EditCompanyAsync(CompanyDTO companyDTO)
        {
            var OldCompany = await _unitOfWork.Repository<Company>().GetByIdAsync(companyDTO.Id);
            if (OldCompany == null)
            {
                return ServiceResponse<CompanyDTO>.ResponseFailure("Company not found.");
            }
            try
            {
                _mapper.Map(companyDTO, OldCompany);
                _unitOfWork.Repository<Company>().Update(OldCompany);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<CompanyDTO>.ResponseSuccess("Company uodated successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<CompanyDTO>.ResponseFailure(ex.Message);
            }
        }
        //public async Task<ServiceResponse<object>> DeleteCompanyByIdAsync(int Id)
        //{

        //}


        public async Task<IEnumerable<DropDownDTO<string>>> SerachCompanyByNameAsync(string Name)
        {
            var companies = await _unitOfWork.Repository<Company>()
                .GetAllAsDtoAsync<DropDownDTO<string>>
                (c => c.Name.ToLower().Contains(Name.ToLower()) && c.IsActive);

            return companies;
        }

        public async Task<GetSearchedCompanyDTO?> GetSearchedCompanyDataAsync(int Id)
        {
            var company = await _unitOfWork.Repository<Company>()
                .GetByIdAsDtoAsync<GetSearchedCompanyDTO>(Id);

            return company;
        }

        public async Task<ServiceResponse<ReservationCompanyDTO>> CreateOrUpdateCompanyAsync(ReservationCompanyDTO companyDTO)
        {
            try
            {
                Company? company = new Company();
                if (companyDTO.Id == 0)
                {
                    company = _mapper.Map<Company>(companyDTO);
                    await _unitOfWork.Repository<Company>().AddNewAsync(company);
                }
                else
                {
                    company = await _unitOfWork.Repository<Company>().GetByIdAsync(companyDTO.Id);

                    _mapper.Map(companyDTO, company);
                    _unitOfWork.Repository<Company>().Update(company);
                }

                await _unitOfWork.CommitAsync();
                var result = _mapper.Map<ReservationCompanyDTO>(company);
                return ServiceResponse<ReservationCompanyDTO>.ResponseSuccess("Company saved successfully.", result);
            }
            catch (Exception ex)
            {
                return ServiceResponse<ReservationCompanyDTO>.ResponseFailure(ex.Message);
            }
        }

    }
}

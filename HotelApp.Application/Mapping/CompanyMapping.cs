using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Company;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Mapping
{
	public class CompanyMapping : Profile
	{
        public CompanyMapping()
        {
            CreateMap<Company, GetAllCompaniesDTO>();

            CreateMap<CompanyDTO, Company>();

            CreateMap<Company, DropDownDTO<string>>()
                .ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));

            CreateMap<Company, CompanyDTO>();
            CreateMap<CompanyDTO, Company>();

            CreateMap<Company, GetSearchedCompanyDTO>();

            CreateMap<ReservationCompanyDTO, Company>();
            CreateMap<Company, ReservationCompanyDTO>();
        }
    }
}

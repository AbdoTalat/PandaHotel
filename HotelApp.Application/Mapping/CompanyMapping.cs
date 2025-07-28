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

            CreateMap<AddCompanyDTO, Company>();

            CreateMap<Company, DropDownDTO<string>>()
                .ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Name));

            CreateMap<Company, EditCompanyDTO>();
            CreateMap<EditCompanyDTO, Company>();
        }
    }
}

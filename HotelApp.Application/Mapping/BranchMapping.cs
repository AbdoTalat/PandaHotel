using AutoMapper;
using HotelApp.Application.DTOs.Branches;
using HotelApp.Application.DTOs.Users;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Mapping
{
    public class BranchMapping : Profile
	{
		public BranchMapping()
		{
			CreateMap<EditBranchDTO, Branch>();
			CreateMap<Branch, EditBranchDTO>();

			CreateMap<Branch, GetBranchByIdDTO>();

			CreateMap<UserBranch, GetBranchesByUserIdDTO>()
				.ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
				.ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name));

			CreateMap<Branch, GetAllBranches>()
				.ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Name))
				.ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.Name))
				.ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.UserName : "N/A"))
				.ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => src.LastModifiedBy != null ? src.LastModifiedBy.UserName : "N/A"));

			CreateMap<Branch, GetBranchItemsDTO>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

		}
    }
}

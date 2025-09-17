using AutoMapper;
using HotelApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Application.DTOs.Rooms;
using HotelApp.Application.DTOs.Users;
using HotelApp.Application.DTOs.Branches;
using HotelApp.Application.DTOs.RoleBased;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.DTOs.Floor;
using HotelApp.Domain.Entities;

namespace HotelApp.Application.Mapping
{
    public class UserRoleMapping : Profile
	{
        public UserRoleMapping()
        {

			//User Mapping
			CreateMap<User, UsersDTO>();

			CreateMap<User, EditUserDTO>()
				.ForMember(dest => dest.AllRoles, opt => opt.Ignore())
				.ForMember(dest => dest.SelectedRoles, opt => opt.Ignore())
				.ForMember(dest => dest.AllBranches, opt => opt.Ignore())
				.ForMember(dest => dest.SelectedBranchIds, opt => opt.Ignore());

			CreateMap<EditUserDTO, User>()
			 .ForMember(dest => dest.Id, opt => opt.Ignore())
			 .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())
			 .ForMember(dest => dest.LastModifiedById, opt => opt.Ignore());

			CreateMap<EditUserDTO, User>();

			CreateMap<UserBranch, DropDownDTO<string>>()
				.ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src => src.Branch.Name))
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Branch.Id));


			//Roles Mapping
			CreateMap<GetRolesDTO, Role>();
			CreateMap<Role, GetRolesDTO>();


			
        }
	}
}
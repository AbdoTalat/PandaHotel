using AutoMapper;
using HotelApp.Domain;
using HotelApp.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelApp.Application.DTOs.Users;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HotelApp.Domain.Entities;
using HotelApp.Application.DTOs.Branches;
using HotelApp.Application.Services.CurrentUserService;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Application.Interfaces;

namespace HotelApp.Application.Services.UserService
{
    public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly ICurrentUserService _currentUserService;


		public UserService(IUnitOfWork unitOfWork, 
			IMapper mapper,
			UserManager<User> userManager,
			 RoleManager<Role> roleManager, 
			 ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userManager = userManager;
			_roleManager = roleManager;
			_currentUserService = currentUserService;

		}

		public async Task<IEnumerable<UsersDTO>> GetAllUsersAsync()
		{
			var users = await _unitOfWork.UserRepository.GetAllUsersAsync();


			return users;
		}
		public async Task<User?> GetUserbyId(int Id)
		{
			return await _userManager.FindByIdAsync(Id.ToString());
		}
		public async Task<IEnumerable<DropDownDTO<string>>> GetUserBranchesByUserIdAsync(int Id)
		{
			var userBranches = await _unitOfWork.UserBranchRepository
                .GetAllAsDtoAsync<DropDownDTO<string>>(ub => ub.UserId == Id, SkipBranchFilter: true);

			return userBranches;
		}
		public async Task<ServiceResponse<EditUserDTO>> GetUserToEditByIdAsync(int Id)
		{
			var user = await _userManager.FindByIdAsync(Id.ToString());
			if (user == null)
			{
				return ServiceResponse<EditUserDTO>.ResponseFailure($"User not found with this ID:{Id}");
			}
			try
			{
				var dto = _mapper.Map<EditUserDTO>(user);

				dto.SelectedBranchIds = (await _unitOfWork.UserRepository
					.GetUserBranchesIDsByUserIdAsync(Id))?.Select(b => (int?)b).ToList() ?? new();

				dto.AllRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
				dto.SelectedRoles = (await _userManager.GetRolesAsync(user)).ToList();

				dto.AllBranches = await _unitOfWork.BranchRepository
					.GetAllAsDtoAsync<DropDownDTO<string>>(SkipBranchFilter: true);

				return ServiceResponse<EditUserDTO>.ResponseSuccess(Data: dto);
			}
			catch (Exception ex)
			{
				return ServiceResponse<EditUserDTO>.ResponseFailure($"Error Occurred: {ex.Message}");
			}
		}
		public async Task<ServiceResponse<AddUserDTO>> AddUserAsync(AddUserDTO userDTO)
		{
			var newUser = new User
			{
				FirstName = userDTO.FirstName,
				LastName = userDTO.LastName,
				UserName = userDTO.UserName,
				Email = userDTO.Email,
				DefaultBranchId = userDTO.SelectedBranchIds.FirstOrDefault(),
				IsActive = userDTO.IsActive,
				CreatedById = _currentUserService.UserId,
				CreatedDate = DateTime.UtcNow
			};			

			try
			{
				var createResult = await _userManager.CreateAsync(newUser, userDTO.Password);
				if (!createResult.Succeeded)
				{
					var errors = createResult.Errors.Select(e => e.Description).ToList();
					return ServiceResponse<AddUserDTO>.ResponseFailure(string.Join(", ", errors));
				}

				var roleResult = await _userManager.AddToRolesAsync(newUser, userDTO.SelectedRoles);
				if (!roleResult.Succeeded)
				{
					var roleErrors = roleResult.Errors.Select(e => e.Description).ToList();
					return ServiceResponse<AddUserDTO>.ResponseFailure(string.Join(", ", roleErrors));
				}

				if (userDTO.SelectedBranchIds != null && userDTO.SelectedBranchIds.Any(b => b.HasValue))
				{
					var newUserBranches = userDTO.SelectedBranchIds
						.Where(b => b.HasValue)
						.Select(branchId => new UserBranch
						{
							UserId = newUser.Id,
							BranchId = branchId.Value,
							AssignedAt = DateTime.UtcNow
						})
						.ToList();
					await _unitOfWork.UserBranchRepository.AddRangeAsync(newUserBranches);
				}
				await _unitOfWork.CommitAsync();

				return ServiceResponse<AddUserDTO>.ResponseSuccess($"New user '{newUser.UserName}' created successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<AddUserDTO>.ResponseFailure($"An exception occurred: {ex.InnerException.Message}");
			}
		}

		
		public async Task<ServiceResponse<EditUserDTO>> EditUserAsync(EditUserDTO userDTO)
		{
			var user = await _userManager.FindByIdAsync(userDTO.Id.ToString());
			if (user == null)
			{
				return ServiceResponse<EditUserDTO>.ResponseFailure($"User with ID: {userDTO.Id} not found");
			}
			try
			{
				_mapper.Map(userDTO, user);
				user.LastModifiedById = _currentUserService.UserId;
				user.LastModifiedDate = DateTime.UtcNow;
				user.DefaultBranchId = userDTO.SelectedBranchIds?.FirstOrDefault();
				var result = await _userManager.UpdateAsync(user);
				if (!result.Succeeded)
				{
					var errors = string.Join(", ", result.Errors.Select(e => e.Description));
					return ServiceResponse<EditUserDTO>.ResponseFailure($"Failed to update user: {errors}");
				}
				// Update roles
				var currentRoles = await _userManager.GetRolesAsync(user);
				await _userManager.RemoveFromRolesAsync(user, currentRoles);
				if (userDTO.SelectedRoles?.Any() == true)
					await _userManager.AddToRolesAsync(user, userDTO.SelectedRoles);

				// Update branches
				var existingBranches = await _unitOfWork.UserBranchRepository
					.GetAllAsync(ub => ub.UserId == userDTO.Id, SkipBranchFilter:true);
				_unitOfWork.UserBranchRepository.DeleteRange(existingBranches);

				if (userDTO.SelectedBranchIds != null && userDTO.SelectedBranchIds.Any(b => b.HasValue))
				{
					var newUserBranches = userDTO.SelectedBranchIds
						.Where(b => b.HasValue)
						.Select(branchId => new UserBranch
						{
							UserId = user.Id,
							BranchId = branchId.Value,
							AssignedAt = DateTime.UtcNow
						})
						.ToList();
					await _unitOfWork.UserBranchRepository.AddRangeAsync(newUserBranches);
				}
				await _unitOfWork.CommitAsync();


				return ServiceResponse<EditUserDTO>.ResponseSuccess("User Updated Successfully");
			}
			catch (Exception ex)
			{
				return ServiceResponse<EditUserDTO>.ResponseFailure($"Error Occurred: {ex.Message}");
			}
		}

		public async Task<ServiceResponse<User>> DeleteUserAsync(int Id)
		{
            var user = await _unitOfWork.UserRepository.GetByIdAsync(Id);
            if (user == null) 
				return ServiceResponse<User>.ResponseFailure($"User with ID:{Id} is not found");

			try
			{
				var userBranches = await _unitOfWork.UserBranchRepository.GetAllAsync(ub => ub.UserId == Id);
				_unitOfWork.UserBranchRepository.DeleteRange(userBranches);
				await _unitOfWork.CommitAsync();
				var result = await _userManager.DeleteAsync(user);
				
				if (result.Succeeded)
				{
					return ServiceResponse<User>.ResponseSuccess("User Deleted successfully.");
				}
				return ServiceResponse<User>.ResponseFailure(string.Join(", ", result.Errors.Select(e => e.Description)));
			}
			catch (Exception ex)
			{
				return ServiceResponse<User>.ResponseFailure(ex.Message);
			}
		}


		public async Task<ServiceResponse<string>> UpdateDefaultBranchId(int BranchId, int userId)
		{
			if (BranchId <= 0)
			{
				return ServiceResponse<string>.ResponseFailure("You need to choose a real Branch");
			}

			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user == null)
			{
				return ServiceResponse<string>.ResponseFailure("you are not Authenticated.");
			}
			user.DefaultBranchId = BranchId;
			try
			{
				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					return ServiceResponse<string>.ResponseSuccess("User Default branch Updated successfully.");
				}	
				return ServiceResponse<string>.ResponseFailure("Somthing went wrong!.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<string>.ResponseFailure(ex.Message);
			}
		}

	}
}

using AutoMapper;
using HotelApp.Domain;
using HotelApp.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelApp.Application.DTOs.Users;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HotelApp.Application.IRepositories;
using HotelApp.Domain.Entities;
using HotelApp.Application.DTOs.Branches;

namespace HotelApp.Application.Services.UserService
{
    public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly IUserRepository _userRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;


		public UserService(IUnitOfWork unitOfWork, 
			IMapper mapper,
			UserManager<User> userManager,
			 RoleManager<Role> roleManager, 
			 IUserRepository userRepository,
			 IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userManager = userManager;
			_roleManager = roleManager;
			_userRepository = userRepository;
			_httpContextAccessor = httpContextAccessor;

		}

		public async Task<IEnumerable<UsersDTO>> GetAllUsersAsync()
		{
			var users = await _userRepository.GetAllUsersAsync();


			return users;
		}
		public async Task<User?> GetUserbyId(int Id)
		{
			return await _userManager.FindByIdAsync(Id.ToString());
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

				dto.SelectedBranchIds = (await _userRepository.GetUserBranchesIDsByUserIdAsync(Id)).ToList();
				dto.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
				dto.SelectedRoles = (await _userManager.GetRolesAsync(user)).ToList();
				dto.AllBranches = await _unitOfWork.Repository<Branch>()
					.GetAllAsDtoAsync<DropDownDTO<string>>(SkipBranchFilter: true);

				return ServiceResponse<EditUserDTO>.ResponseSuccess("Success",dto);
			}
			catch (Exception ex)
			{
				return ServiceResponse<EditUserDTO>.ResponseFailure($"Error Occurred: {ex.Message}");
			}
		}
		public async Task<ServiceResponse<User>> AddUserAsync(AddUserDTO userDTO)
		{
			var newUser = new User
			{
				FirstName = userDTO.FirstName,
				LastName = userDTO.LastName,
				UserName = userDTO.userName,
				Email = userDTO.email,
				DefaultBranchId = userDTO.DefaultBranchId,
				isActive = userDTO.IsActive,
				CreatedById = GetCurrentUserId(),
				CreatedDate = DateTime.UtcNow
			};			

			try
			{
				var createResult = await _userManager.CreateAsync(newUser, userDTO.password);
				if (!createResult.Succeeded)
				{
					var errors = createResult.Errors.Select(e => e.Description).ToList();
					return ServiceResponse<User>.ResponseFailure(string.Join(", ", errors));
				}

				if (userDTO.RoleId > 0)
				{
					var role = await _roleManager.FindByIdAsync(userDTO.RoleId.ToString());
					if (role == null)
					{
						return ServiceResponse<User>.ResponseFailure($"Role with ID {userDTO.RoleId} not found.");
					}

					var roleResult = await _userManager.AddToRoleAsync(newUser, role.Name);
					if (!roleResult.Succeeded)
					{
						var roleErrors = roleResult.Errors.Select(e => e.Description).ToList();
						return ServiceResponse<User>.ResponseFailure(string.Join(", ", roleErrors));
					}
				}
				var newUserBranch = new UserBranch
				{
					UserId = newUser.Id,
					BranchId = newUser.DefaultBranchId,
					AssignedAt = DateTime.UtcNow
				};
				await _unitOfWork.Repository<UserBranch>().AddNewAsync(newUserBranch);
				await _unitOfWork.CommitAsync();

				return ServiceResponse<User>.ResponseSuccess($"New user '{newUser.UserName}' created successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<User>.ResponseFailure($"An exception occurred: {ex.InnerException.Message}");
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
				user.LastModifiedById = GetCurrentUserId();
				user.LastModifiedDate = DateTime.UtcNow;
				await _userManager.UpdateAsync(user);

				// Update roles
				var currentRoles = await _userManager.GetRolesAsync(user);
				await _userManager.RemoveFromRolesAsync(user, currentRoles);
				await _userManager.AddToRolesAsync(user, userDTO.SelectedRoles);

				// Update branches
				var existingBranches = await _unitOfWork.Repository<UserBranch>()
					.GetAllAsync(ub => ub.UserId == userDTO.Id, SkipBranchFilter:true);
				_unitOfWork.Repository<UserBranch>().DeleteRange(existingBranches);

				var newUserBranches = userDTO.SelectedBranchIds.Select(branchId => new UserBranch
				{
					UserId = user.Id,
					BranchId = branchId,
					AssignedAt = DateTime.UtcNow
				}).ToList();

				await _unitOfWork.Repository<UserBranch>().AddRangeAsync(newUserBranches);
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
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(Id);
            if (user == null) 
				return ServiceResponse<User>.ResponseFailure($"User with ID:{Id} is not found");

			try
			{
				var userBranches = await _unitOfWork.Repository<UserBranch>().GetAllAsync(ub => ub.UserId == Id);
				_unitOfWork.Repository<UserBranch>().DeleteRange(userBranches);
				await _unitOfWork.CommitAsync();
				var result = await _userManager.DeleteAsync(user);
				
				if (result.Succeeded)
				{
					return ServiceResponse<User>.ResponseSuccess("User Deleted successfully.");
				}
				return ServiceResponse<User>.ResponseFailure(result.Errors.FirstOrDefault().ToString());
			}
			catch (Exception ex)
			{
				return ServiceResponse<User>.ResponseFailure(ex.InnerException.Message);
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

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				return ServiceResponse<string>.ResponseSuccess("User Default branch Updated successfully.");
			}
			return ServiceResponse<string>.ResponseFailure("Somthing went wrong!.");
		}


		#region Helper Methods For User
		private int GetCurrentUserId()
		{
			if (int.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
			{
				return userId;
			}

			return 0;
		}
		#endregion
	}
}

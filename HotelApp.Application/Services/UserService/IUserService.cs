using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Users;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.UserService
{
    public interface IUserService
	{
		Task<IEnumerable<UsersDTO>> GetAllUsersAsync();
		Task<User?> GetUserbyId(int Id);
		Task<ServiceResponse<User>> AddUserAsync(AddUserDTO userDTO);
		Task<ServiceResponse<EditUserDTO>> GetUserToEditByIdAsync(int Id);
		Task<ServiceResponse<EditUserDTO>> EditUserAsync(EditUserDTO model);
		Task<ServiceResponse<User>> DeleteUserAsync(int Id);
		Task<ServiceResponse<string>> UpdateDefaultBranchId(int BranchId, int userId);

	}
}

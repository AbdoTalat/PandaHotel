using HotelApp.Application.DTOs;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Interfaces.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IEnumerable<UsersDTO>> GetAllUsersAsync();
        Task<IEnumerable<int>> GetUserBranchesIDsByUserIdAsync(int Id);
    }
}

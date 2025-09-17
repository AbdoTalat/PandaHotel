using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Branches;
using HotelApp.Application.IRepositories;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.BranchService
{
    public interface IBranchService
    {
		Task<GetBranchByIdDTO?> GetBranchByIdAsync(int Id);
		Task<IEnumerable<DropDownDTO<string>>> GetBranchsDropDownAsync();
		Task<IEnumerable<GetAllBranches>> GetAllBranchesAsync();
		Task<IEnumerable<GetBranchesByUserIdDTO>> GetBranchesByUserId(int userId);
		Task<ServiceResponse<BranchDTO>> AddNewBranchAsync(BranchDTO branchDTO);
		Task<BranchDTO?> GetBranchToEditByIdAsync(int Id);
		Task<ServiceResponse<BranchDTO>> EditBranchAsync(BranchDTO branchDTO);
		Task<ServiceResponse<Branch>> DeleteBranchAsync(int Id);
		
	}
}

using AutoMapper;
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
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		private readonly IUserRepository _userRepository;

		public BranchService(IUnitOfWork unitOfWork, IMapper mapper, 
             IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
			_userRepository = userRepository;
		}
		
		public async Task<GetBranchByIdDTO?> GetBranchByIdAsync(int Id)
        {
            var branch = await _unitOfWork.Repository<Branch>()
                .GetByIdAsDtoAsync<GetBranchByIdDTO>(Id, SkipBranchFilter:true);

            return branch;
		}
		public async Task<IEnumerable<DropDownDTO<string>>> GetBranchsDropDownAsync()
        {
            var branches = await _unitOfWork.Repository<Branch>().GetAllAsDtoAsync<DropDownDTO<string>>(SkipBranchFilter:true);
            return branches;
        }
		public async Task<IEnumerable<GetAllBranches>> GetAllBranchesAsync()
        {
            var branches = await _unitOfWork.Repository<Branch>().GetAllAsDtoAsync<GetAllBranches>();
            return branches;
        }
        public async Task<IEnumerable<GetBranchesByUserIdDTO>> GetBranchesByUserId(int userId)
        {
            var branches = await _unitOfWork.Repository<UserBranch>()
                .GetAllAsDtoAsync<GetBranchesByUserIdDTO>(ub => ub.UserId == userId, SkipBranchFilter:true);
            return branches;
        }
        public async Task<ServiceResponse<BranchDTO>> AddNewBranchAsync(BranchDTO branchDTO)
        {
            try
            {
                var mappedBranch = _mapper.Map<Branch>(branchDTO);
                await _unitOfWork.Repository<Branch>().AddNewAsync(mappedBranch);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<BranchDTO>.ResponseSuccess($"{mappedBranch.Name} Branch Created Succesfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<BranchDTO>.ResponseFailure($"Error Occurred: {ex.Message}");
            }
        }
		public async Task<ServiceResponse<BranchDTO>> EditBranchAsync(BranchDTO branchDTO)
        {
            var oldBranch = await _unitOfWork.Repository<Branch>().GetByIdAsync(branchDTO.Id);
            if(oldBranch == null)
            {
                return ServiceResponse<BranchDTO>.ResponseFailure("Branch not found.");
            }
            _mapper.Map(branchDTO, oldBranch);

            try
            {
                _unitOfWork.Repository<Branch>().Update(oldBranch);
                await _unitOfWork.CommitAsync();
				return ServiceResponse<BranchDTO>.ResponseSuccess("Branch Updated Successfully.");
			}
            catch (Exception ex)
            {
                return ServiceResponse<BranchDTO>.ResponseFailure($"Error Occurred: {ex.Message}");
            }
		}
		public async Task<ServiceResponse<Branch>> DeleteBranchAsync(int Id)
        {
            try
            {
                var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(Id);
                if (branch == null)
                {
                    return ServiceResponse<Branch>.ResponseFailure("Branch not found");

                }
                _unitOfWork.Repository<Branch>().Delete(branch);
                await _unitOfWork.CommitAsync();
                return ServiceResponse<Branch>.ResponseSuccess("Branch Deleted Successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Branch>.ResponseFailure($"Error Occurred: {ex.Message}");
            }
        }
		public async Task<BranchDTO?> GetBranchToEditByIdAsync(int Id)
		{
			var branch = await _unitOfWork.Repository<Branch>().GetByIdAsDtoAsync<BranchDTO>(Id);
            return branch;
		}
	}
}

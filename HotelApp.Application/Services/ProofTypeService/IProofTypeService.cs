using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.ProofType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.ProofTypeService
{
    public interface IProofTypeService
    {
        Task<IEnumerable<DropDownDTO<string>>> GetProofTypesDropDownAsync();
        Task<IEnumerable<ProofTypeDTO>> GetAllProofTypesAsync();
		Task<ServiceResponse<ProofTypeDTO>> AddProofTypeAsync(ProofTypeDTO dto);	
		Task<ProofTypeDTO?> GetProofTypeToEditByIdAsync(int Id);
		Task<ServiceResponse<ProofTypeDTO>> EditProofTypeAsync(ProofTypeDTO dto);
		Task<ServiceResponse<object>> DeleteProofTypeAsync(int Id);
	}
}

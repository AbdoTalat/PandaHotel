using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.ProofType;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.ProofTypeService
{
    public class ProofTypeService : IProofTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProofTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DropDownDTO<string>>> GetProofTypesDropDownAsync()
        {
            var proofTypes = await _unitOfWork.Repository<ProofType>()
                .GetAllAsDtoAsync<DropDownDTO<string>>();

            return proofTypes;
        }

        public async Task<IEnumerable<GetAllProofTypesDTO>> GetAllProofTypesAsync()
        {
            var ProofTypes = await _unitOfWork.Repository<ProofType>()
                .GetAllAsDtoAsync<GetAllProofTypesDTO>();

            return ProofTypes;
        }
	}
}

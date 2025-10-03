using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.ProofType;
using HotelApp.Application.Interfaces;
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
		private readonly IMapper _mapper;

		public ProofTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
		}

        public async Task<IEnumerable<DropDownDTO<string>>> GetProofTypesDropDownAsync()
        {
            var proofTypes = await _unitOfWork.ProofTypeRepository
                .GetAllAsDtoAsync<DropDownDTO<string>>();

            return proofTypes;
        }

        public async Task<IEnumerable<ProofTypeDTO>> GetAllProofTypesAsync()
        {
            var ProofTypes = await _unitOfWork.ProofTypeRepository
                .GetAllAsDtoAsync<ProofTypeDTO>();

            return ProofTypes;
        }

        public async Task<ServiceResponse<ProofTypeDTO>> AddProofTypeAsync(ProofTypeDTO dto)
        {
            try
            {
                var ProofType = _mapper.Map<ProofType>(dto);
                await _unitOfWork.ProofTypeRepository.AddNewAsync(ProofType);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<ProofTypeDTO>.ResponseSuccess("New Proof Type added successfully.");
            }
            catch (Exception ex)
            {
				return ServiceResponse<ProofTypeDTO>.ResponseFailure(ex.Message);
			}
        }
        public async Task<ProofTypeDTO?> GetProofTypeToEditByIdAsync(int Id)
        {
            var ProofType = await _unitOfWork.ProofTypeRepository
                .GetByIdAsDtoAsync<ProofTypeDTO>(Id);

            return ProofType;
        }
		public async Task<ServiceResponse<ProofTypeDTO>> EditProofTypeAsync(ProofTypeDTO dto)
        {
            var OldProofType = await _unitOfWork.ProofTypeRepository
                .GetByIdAsync(dto.Id);
            if (OldProofType == null)
            {
                return ServiceResponse<ProofTypeDTO>.ResponseFailure($"No Proof Type with this ID {dto.Id}");
            }
            try
            {
                _mapper.Map(dto, OldProofType);

                _unitOfWork.ProofTypeRepository.Update(OldProofType);
                await _unitOfWork.CommitAsync();

				return ServiceResponse<ProofTypeDTO>.ResponseSuccess("Proof Type Updated Successfully.");
			}
            catch (Exception ex)
            {
				return ServiceResponse<ProofTypeDTO>.ResponseFailure(ex.Message);
			}

        }
		public async Task<ServiceResponse<object>> DeleteProofTypeAsync(int Id)
        {
            bool IsExist = await _unitOfWork.ProofTypeRepository
                .IsExistsAsync(pt => pt.Id == Id);
            if (!IsExist)
            {
				return ServiceResponse<object>.ResponseFailure($"No Proof Type with this ID {Id}");
			}
            try
            {
                await _unitOfWork.ProofTypeRepository.DeleteByIdAsync(Id);
                await _unitOfWork.CommitAsync();
                return ServiceResponse<object>.ResponseSuccess("Proof Type Deleted Successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<object>.ResponseFailure(ex.Message);
            }
		}
	}
}

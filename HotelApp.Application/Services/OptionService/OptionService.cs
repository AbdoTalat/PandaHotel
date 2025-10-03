using AutoMapper;
using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Options;
using HotelApp.Application.Interfaces;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.OptionService
{
    public class OptionService : IOptionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public OptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<DropDownDTO<string>>> GetOptionsDropDownAsync()
		{
			var options = await _unitOfWork.OptionRepository.GetAllAsDtoAsync<DropDownDTO<string>>();
			return options;
		}

		public async Task<IEnumerable<GetAllOptionsDTO>> GetAllOptionsAsync()
		{
			var options = await _unitOfWork.OptionRepository.GetAllAsDtoAsync<GetAllOptionsDTO>();

			return options;
		}
		public async Task<OptionDTO?> GetOptionToEditByIdAsync(int Id)
		{
			var option = await _unitOfWork.OptionRepository.GetByIdAsDtoAsync<OptionDTO>(Id);

			return option;
		}
        public async Task<ServiceResponse<OptionDTO>> AddOptionAsync(OptionDTO optionDTO)
		{
			try
			{
				var option = _mapper.Map<Option>(optionDTO);

				await _unitOfWork.OptionRepository.AddNewAsync(option);
				await _unitOfWork.CommitAsync();

				return ServiceResponse<OptionDTO>.ResponseSuccess("New Option added successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<OptionDTO>.ResponseFailure(ex.Message);
			}
		}

		public async Task<ServiceResponse<OptionDTO>> EditOptionAsync(OptionDTO optionDTO)
		{
			var oldOption = await _unitOfWork.OptionRepository.GetByIdAsync(optionDTO.Id);

			if (oldOption == null)
			{
				return ServiceResponse<OptionDTO>.ResponseFailure("Option not found.");
			}

			try
			{
				_mapper.Map(optionDTO, oldOption);

				_unitOfWork.OptionRepository.Update(oldOption);
				await _unitOfWork.CommitAsync();

				return ServiceResponse<OptionDTO>.ResponseSuccess("Option updated successfully");
			}
			catch (Exception ex)
			{
				return ServiceResponse<OptionDTO>.ResponseFailure(ex.Message);
			}
		}

		public async Task<ServiceResponse<Option>> DeleteOptionAsync(int Id)
		{
			var option = await _unitOfWork.OptionRepository.GetByIdAsync(Id);

			if (option == null)
			{
				return ServiceResponse<Option>.ResponseFailure("Option not found.");
			}

			var isUsed = await _unitOfWork.RoomOptionRepository.IsExistsAsync(ro => ro.OptionId == Id);

			if (isUsed)
			{
				return ServiceResponse<Option>.ResponseFailure("This option is used in room.");
			}
			try
			{
				_unitOfWork.OptionRepository.Delete(option);
				await _unitOfWork.CommitAsync();

				return ServiceResponse<Option>.ResponseSuccess("Option deleted successfully");
			}
			catch (Exception ex)
			{
				return ServiceResponse<Option>.ResponseFailure(ex.Message);
			}
		}
    }
}

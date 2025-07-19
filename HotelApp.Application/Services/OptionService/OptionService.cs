using AutoMapper;
using HotelApp.Application.DTOs.Options;
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

		public async Task<IEnumerable<GetOptionItemsDTO>> GetOptionItemsAsync()
		{
			var options = await _unitOfWork.Repository<Option>().GetAllAsDtoAsync<GetOptionItemsDTO>();
			return options;
		}

		public async Task<IEnumerable<GetAllOptionsDTO>> GetAllOptionsAsync()
		{
			var options = await _unitOfWork.Repository<Option>().GetAllAsDtoAsync<GetAllOptionsDTO>();

			return options;
		}
		public async Task<EditOptionDTO?> GetOptionToEditByIdAsync(int Id)
		{
			var option = await _unitOfWork.Repository<Option>().GetByIdAsDtoAsync<EditOptionDTO>(Id);

			return option;
		}
        public async Task<ServiceResponse<AddOptionDTO>> AddOptionAsync(AddOptionDTO optionDTO)
		{
			try
			{
				var option = _mapper.Map<Option>(optionDTO);

				await _unitOfWork.Repository<Option>().AddNewAsync(option);
				await _unitOfWork.CommitAsync();

				return ServiceResponse<AddOptionDTO>.ResponseSuccess("New Option added successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<AddOptionDTO>.ResponseFailure(ex.Message);
			}
		}

		public async Task<ServiceResponse<EditOptionDTO>> EditOptionAsync(EditOptionDTO optionDTO)
		{
			var oldOption = await _unitOfWork.Repository<Option>().GetByIdAsync(optionDTO.Id);

			if (oldOption == null)
			{
				return ServiceResponse<EditOptionDTO>.ResponseFailure("Option not found.");
			}

			try
			{
				_mapper.Map(optionDTO, oldOption);

				_unitOfWork.Repository<Option>().Update(oldOption);
				await _unitOfWork.CommitAsync();

				return ServiceResponse<EditOptionDTO>.ResponseSuccess("Option updated successfully");
			}
			catch (Exception ex)
			{
				return ServiceResponse<EditOptionDTO>.ResponseFailure(ex.Message);
			}
		}

		public async Task<ServiceResponse<Option>> DeleteOptionAsync(int Id)
		{
			var option = await _unitOfWork.Repository<Option>().GetByIdAsync(Id);

			if (option == null)
			{
				return ServiceResponse<Option>.ResponseFailure("Option not found.");
			}

			var isUsed = await _unitOfWork.Repository<RoomOption>().IsExistsAsync(ro => ro.OptionId == Id);

			if (isUsed)
			{
				return ServiceResponse<Option>.ResponseFailure("This option is used in room.");
			}
			try
			{
				_unitOfWork.Repository<Option>().Delete(option);
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

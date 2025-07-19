using AutoMapper;
using HotelApp.Application.DTOs.Guests;
using HotelApp.Application.IRepositories;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Application.Services.GuestService
{
    public class GuestService : IGuestService
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGuestRepository _guestRepository;

		public GuestService(IUnitOfWork unitOfWork, IMapper mapper, IGuestRepository guestRepository)
        {
			_unitOfWork = unitOfWork;
            _mapper = mapper;
            _guestRepository = guestRepository;
		}


		public async Task<IEnumerable<GetAllGuestsDTO>> GetAllGuestsAsync()
		{
			var guests = await _unitOfWork.Repository<Guest>().GetAllAsDtoAsync<GetAllGuestsDTO>();
			return guests;
		}
		public async Task<GetGuestByIdDTO?> GetGuestByIdAsync(int Id)
		{
			var guest = await _unitOfWork.Repository<Guest>().GetByIdAsDtoAsync<GetGuestByIdDTO>(Id);
			return guest;
        }
		public async Task<EditGuestDTO?> GetGuestToEditByIdAsync(int Id)
		{
			var guest = await _unitOfWork.Repository<Guest>().GetByIdAsDtoAsync<EditGuestDTO>(Id);

			return guest;
		}
		public async Task<ServiceResponse<AddGuestDTO>> AddGuestAsync(AddGuestDTO guestDTO)
		{
			try
			{
				var checkGuestExist = await _unitOfWork.Repository<Guest>().IsExistsAsync(g => g.Phone == guestDTO.Phone); ;
				if (checkGuestExist)
				{
					return ServiceResponse<AddGuestDTO>.ResponseFailure($"There is already guest with phone number: {guestDTO.Phone}");
				}
				var mappedGuest = _mapper.Map<Guest>(guestDTO);
				if (mappedGuest == null)
				{
                    return ServiceResponse<AddGuestDTO>.ResponseFailure($"Error Occurred: Guest failed at mapping");
                } 
				await _unitOfWork.Repository<Guest>().AddNewAsync(mappedGuest);
				await _unitOfWork.CommitAsync();

				return ServiceResponse<AddGuestDTO>.ResponseSuccess("Guest Added Successfully");
			}
			catch (Exception ex)
			{
                return ServiceResponse<AddGuestDTO>.ResponseFailure($"Error Occurred: {ex.Message}");
            }
        }
        public async Task<ServiceResponse<EditGuestDTO>> EditGuestAsync(EditGuestDTO guestDTO, int Id)
		{
			var OldGuest = await _unitOfWork.Repository<Guest>().GetByIdAsync(Id);
			if (OldGuest == null)
			{
                return ServiceResponse<EditGuestDTO>.ResponseFailure($"Guest with ID: {Id} not found");
            }
			try
			{
				var mappedGuest = _mapper.Map(guestDTO, OldGuest);
				_unitOfWork.Repository<Guest>().Update(mappedGuest);
				await _unitOfWork.CommitAsync();
				return ServiceResponse<EditGuestDTO>.ResponseSuccess("Guest Updated Successfully");
			}
			catch(Exception ex)
			{
				return ServiceResponse<EditGuestDTO>.ResponseFailure(ex.Message);
			}
		}
        public async Task<ServiceResponse<Guest>> DeleteGuestAsync(int Id)
		{
			var guest = await _unitOfWork.Repository<Guest>().GetByIdAsync(Id);
			if (guest == null)
			{
				return ServiceResponse<Guest>.ResponseFailure($"Guest with ID: {Id} not found");
			}
			try
			{
				_unitOfWork.Repository<Guest>().Delete(guest);
				await _unitOfWork.CommitAsync();
				return ServiceResponse<Guest>.ResponseSuccess("Guest Deleted Successfully");
			}
			catch (Exception ex)
			{
				return ServiceResponse<Guest>.ResponseFailure($"Error Occurred: {ex.InnerException.Message}");
			}
		}
		public async Task<IEnumerable<GetSearchedGuestsDTO>> SerachGuestsByEmailAsync(string email)
		{
			var guest = await _guestRepository.SerachGuestsByEmailAsync(email);
			return guest;
		}
	}
}

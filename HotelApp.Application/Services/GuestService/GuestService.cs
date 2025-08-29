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
			var guest = await _unitOfWork.Repository<Guest>()
				.GetByIdAsDtoAsync<GetGuestByIdDTO>(Id);
			return guest;
        }
		public async Task<GuestDTO?> GetGuestToEditByIdAsync(int Id)
		{
			var guest = await _unitOfWork.Repository<Guest>().GetByIdAsDtoAsync<GuestDTO>(Id);

			return guest;
		}
		public async Task<ServiceResponse<GuestDTO>> AddGuestAsync(GuestDTO guestDTO)
		{
			var checkGuestPhoneExist = await _unitOfWork.Repository<Guest>().IsExistsAsync(g => g.Phone.Contains(guestDTO.Phone));
			if (checkGuestPhoneExist)
				return ServiceResponse<GuestDTO>.ResponseFailure($"There is already guest with phone number: {guestDTO.Phone}");
			
			var checkGuestEmailExist = await _unitOfWork.Repository<Guest>().IsExistsAsync(g => g.Email.Contains(guestDTO.Email));
			if (checkGuestEmailExist)
				return ServiceResponse<GuestDTO>.ResponseFailure($"There is already guest with Email Address: {guestDTO.Phone}");
			
			try
			{
				var mappedGuest = _mapper.Map<Guest>(guestDTO);
				if (mappedGuest == null)
				{
					return ServiceResponse<GuestDTO>.ResponseFailure($"Error Occurred: Guest failed at mapping");
				}
				await _unitOfWork.Repository<Guest>().AddNewAsync(mappedGuest);
				await _unitOfWork.CommitAsync();

				return ServiceResponse<GuestDTO>.ResponseSuccess("Guest Added Successfully.");
			}
			catch (Exception ex)
			{
				return ServiceResponse<GuestDTO>.ResponseFailure($"Error Occurred: {ex.Message}");
			}
		}
        public async Task<ServiceResponse<GuestDTO>> EditGuestAsync(GuestDTO dto)
		{
			var OldGuest = await _unitOfWork.Repository<Guest>().GetByIdAsync(dto.Id);
			if (OldGuest == null)
			{
                return ServiceResponse<GuestDTO>.ResponseFailure($"Guest with ID: {dto.Id} not found");
            }
			try
			{
				var mappedGuest = _mapper.Map(dto, OldGuest);
				_unitOfWork.Repository<Guest>().Update(mappedGuest);
				await _unitOfWork.CommitAsync();
				return ServiceResponse<GuestDTO>.ResponseSuccess("Guest Updated Successfully");
			}
			catch(Exception ex)
			{
				return ServiceResponse<GuestDTO>.ResponseFailure(ex.InnerException.Message);
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

		public async Task<ServiceResponse<ReservationGuestDTO>> AddOrEditGuestsAsync(GuestDTO guestDTO)
		{
			try
			{
				var guest = _mapper.Map<Guest>(guestDTO);

				if (guest.Id == 0)
				{
					var IsGuestExists = await _unitOfWork.Repository<Guest>()
					.IsExistsAsync(g =>
					   g.FullName.Contains(guestDTO.FullName)
					|| g.Email.Contains(guestDTO.Email) 
					|| g.Phone.Contains(guestDTO.Phone));

					if (IsGuestExists)
					{
						return ServiceResponse<ReservationGuestDTO>.ResponseFailure("This guest already exists.");
					}
					await _unitOfWork.Repository<Guest>().AddNewAsync(guest);
				}
				else
				{
					var oldGuest = await _unitOfWork.Repository<Guest>().GetByIdAsync(guestDTO.Id);
					_mapper.Map(guestDTO, oldGuest);
					_unitOfWork.Repository<Guest>().Update(oldGuest);
				}
				await _unitOfWork.CommitAsync();

				var addedGuest = await _unitOfWork.Repository<Guest>()
					.GetByIdAsDtoAsync<ReservationGuestDTO>(guest.Id);

				if (addedGuest == null)
				{
					return ServiceResponse<ReservationGuestDTO>.ResponseFailure("guest was not added");
				}

				addedGuest.IsPrimary = guestDTO.IsPrimary;

				return ServiceResponse<ReservationGuestDTO>.ResponseSuccess(Data: addedGuest);
			}
			catch (Exception ex)
			{
				return ServiceResponse<ReservationGuestDTO>.ResponseFailure(ex.Message);
			}

		}


		public async Task<IEnumerable<GetSearchedGuestsDTO>> SearchGuestsAsync(string term)
		{
			var guetss = await _guestRepository.SearchGuestsAsync(term);

			return guetss;
		}
    }
}

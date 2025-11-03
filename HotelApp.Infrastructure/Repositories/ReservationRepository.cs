using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelApp.Application.DTOs.Reservation;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Domain.Entities;
using HotelApp.Domain.Enums;
using HotelApp.Helper;
using HotelApp.Infrastructure.DbContext;
using HotelApp.Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HotelApp.Infrastructure.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;

        public ReservationRepository(ApplicationDbContext context, 
			IConfigurationProvider mapperConfig)
            : base(context, mapperConfig)
        {
            _context = context;
            _mapperConfig = mapperConfig;
        }
		public async Task<ReservationDTO> GetReservationToEditByIdAsync(int Id)
		{
			var dto = await _context.Reservations
				.AsNoTracking()
				.BranchFilter()
				.Where(r => r.Id == Id)
				.Select(r => new ReservationDTO
				{
					ReservationInfoDto = new ReservationInfoDTO
					{
						ReservationId = r.Id,
						ReservationNumber = r.ReservationNumber,
						CheckInDate = r.CheckInDate,
						CheckOutDate = r.CheckOutDate,
						NumOfNights = r.NumberOfNights,
						ReservationSourceId = r.ReservationSourceId,
						CompanyId = r.CompanyId,
						RateId = (int)r.RateId,
						RoomsIDs = r.ReservationsRooms.Select(rr => rr.RoomId).ToList(),
						RoomTypeToBookDTOs = r.ReservationRoomTypes
							.Select(rt => new RoomTypeToBookDTO
							{
								RoomTypeId = rt.RoomTypeId,
								NumOfRooms = rt.Quantity,
								NumOfAdults = rt.NumOfAdults,
								NumOfChildrens = rt.NumOfChildren
							})
							.ToList()
					},
					GuestDtos = r.guestReservations
						.Select(gr => new ReservationGuestDTO
						{
							GuestId = gr.GuestId,
							IsPrimary = gr.IsPrimaryGuest,
							FullName = gr.Guest.FullName,
							Phone = gr.Guest.Phone,
							Email = gr.Guest.Email,
							ProofNumber = gr.Guest.ProofNumber,
							ProofTypeId = gr.Guest.ProofTypeId,
							Address = gr.Guest.Address,
							DateOfBirth = gr.Guest.DateOfBirth
						}).ToList(),

                    ConfirmDto = new ConfirmReservationDTO
                    {
                        IsPending = r.ReservationHistories
                    .Any(h => h.Status == ReservationStatus.Pending),
                        IsConfirmed = r.ReservationHistories
                    .Any(h => h.Status == ReservationStatus.Confirmed),
                        IsCheckedIn = r.ReservationHistories
                    .Any(h => h.Status == ReservationStatus.CheckedIn),
                        IsCheckedOut = r.ReservationHistories
                    .Any(h => h.Status == ReservationStatus.CheckedOut),
                        IsCancelled = r.ReservationHistories
                    .Any(h => h.Status == ReservationStatus.Cancelled),
                        IsNoShow = r.ReservationHistories
                    .Any(h => h.Status == ReservationStatus.NoShow),
                        Comment = r.Notes,
                        CancellationReason = r.CancellationReason
                    }
                })
				.FirstOrDefaultAsync();

			return dto ?? new ReservationDTO();
		}
        public async Task<Reservation> GetReservationDetailsByIds(int Id)
		{
			var reservation = await _context.Reservations
				.BranchFilter()
				.Where(r => r.Id == Id)
				.Include(r => r.ReservationSource)
				.Include(r => r.Company)
				.Include(r => r.ReservationsRooms)
					.ThenInclude(r => r.Room)
				.Include(r => r.guestReservations)
					.ThenInclude(gr => gr.Guest)
				.Include(r => r.ReservationRoomTypes)
					.ThenInclude(rt => rt.RoomType)
				.FirstOrDefaultAsync();

			return reservation;
		}
        public async Task<IEnumerable<GetAllReservationsDTO>> GetFilteredReservationsAsync(ReservationFilterDTO dto)
        {
            var today = GetEgyptDateTime().Date;
            var tomorrow = today.AddDays(1);

            var query = _context.Reservations
                .AsNoTracking()
                .BranchFilter()
                .AsQueryable();

            if (dto.ReservationCategory.HasValue && dto.ReservationCategory.Value != ReservationCategory.All)
            {
                switch (dto.ReservationCategory.Value)
                {
                    case ReservationCategory.NewBookings:
                        query = query.Where(r =>
                            r.Status != ReservationStatus.Cancelled &&
                            r.CreatedDate.HasValue &&
                            r.CreatedDate.Value >= today && r.CreatedDate.Value < tomorrow);
                        break;

                    case ReservationCategory.Arrivals:
                        query = query.Where(r =>
                            r.Status == ReservationStatus.Confirmed &&
                            r.CheckInDate >= today && r.CheckInDate < tomorrow);
                        break;

                    case ReservationCategory.Departures:
                        query = query.Where(r =>
                            r.Status != ReservationStatus.Cancelled &&
                            r.CheckOutDate >= today && r.CheckOutDate < tomorrow);
                        break;

                    case ReservationCategory.StayOvers:
                        query = query.Where(r =>
                            r.Status == ReservationStatus.CheckedIn &&
                            r.CheckInDate <= today && r.CheckOutDate > today);
                        break;

                    case ReservationCategory.Cancellations:
                        query = query.Where(r =>
                            r.Status == ReservationStatus.Cancelled &&
                            r.LastModifiedDate.HasValue &&
                            r.LastModifiedDate.Value >= today && r.LastModifiedDate.Value < tomorrow);
                        break;

                    case ReservationCategory.NoShow:
                        query = query.Where(r =>
                            r.Status == ReservationStatus.NoShow &&
                            r.CheckInDate >= today && r.CheckInDate < tomorrow);
                        break;
                }
            }

            if (dto.CheckInDate.HasValue)
                query = query.Where(r => r.CheckInDate >= dto.CheckInDate.Value.Date);

            if (dto.CheckOutDate.HasValue)
                query = query.Where(r => r.CheckOutDate <= dto.CheckOutDate.Value.Date);

            if (!string.IsNullOrWhiteSpace(dto.PrimaryGuestName))
            {
                query = query.Where(r => r.guestReservations.Any(gr =>
                    gr.IsPrimaryGuest &&
                    gr.Guest.FullName.Contains(dto.PrimaryGuestName)));
            }

            return await query
                .ProjectTo<GetAllReservationsDTO>(_mapperConfig)
                .ToListAsync();
        }

        public async Task<GetReservationDetailsByIdDTO> GetReservationDetailsByIdAsync(int Id)
        {
            var reservation = await _context.Reservations
                .AsNoTracking()
                .BranchFilter()
                .Where(r => r.Id == Id)
				.Include(r => r.Rate)
                .Include(r => r.ReservationSource)
                .Include(r => r.ReservationRoomTypes)
                .Include(r => r.ReservationsRooms)
                    .ThenInclude(rr => rr.Room)
                .Include(r => r.guestReservations)
                    .ThenInclude(gr => gr.Guest)
                .Include(r => r.ReservationHistories)
                    .ThenInclude(h => h.PerformedBy)
                .Select(r => new GetReservationDetailsByIdDTO
                {
                    Id = r.Id,
                    ReservationNumber = r.ReservationNumber,
                    CheckInDate = r.CheckInDate,
                    CheckOutDate = r.CheckOutDate,
                    RateCode = r.Rate.Code,
                    Status = r.Status,
                    NumberOfNights = r.NumberOfNights,
                    NumberOfPeople = r.NumberOfPeople,
                    TotalPrice = r.TotalPrice,
                    ReservationSource = r.ReservationSource.Name,
                    Notes = r.Notes,
                    CreatedBy = r.CreatedBy.UserName,
                    LastModifiedBy = r.LastModifiedBy.UserName,
                    NumOfTotalRooms = r.ReservationsRooms.Count(),

                    GuestReservations = r.guestReservations.Select(gr => new ReservationDetailsGuestsDTO
                    {
                        Id = gr.Guest.Id,
                        IsPrimaryGuest = gr.IsPrimaryGuest,
                        FullName = gr.Guest.FullName,
                        Phone = gr.Guest.Phone,
                        Email = gr.Guest.Email
                    }).ToList(),

                    ReservationRoomTypes = r.ReservationRoomTypes.Select(rt => new ReservationDetailsRoomTypeDTO
                    {
                        Id = rt.Id,
                        RoomTypeName = rt.RoomType.Name,
                        Quantity = rt.Quantity,
                        NumOfAdults = rt.NumOfAdults,
                        NumOfChildren = rt.NumOfChildren
                    }).ToList(),

                    ReservationRooms = r.ReservationsRooms.Select(rr => new ReservationDetailsRoomsDTO
                    {
                        RoomNumber = rr.Room.RoomNumber,
                        RoomTypeId = rr.Room.RoomTypeId
                    }).ToList(),

                    ReservationHistories = r.ReservationHistories.Select(h => new ReservationDetailsHistoryDTO
                    {
                        PerformedByName = h.PerformedBy.Email,
                        PerformedDate = h.PerformedDate,
                        Status = h.Status.ToString()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return reservation;
        }

		public async Task<CheckOutDTO> GetCheckOutReservationByIdAsync(int Id)
		{
			var dto = await _context.Reservations.
				BranchFilter()
				.Where(r => r.Id == Id)
				.Select(r => new CheckOutDTO
				{
					Id = r.Id,
					ReservationNumber = r.ReservationNumber ?? "N",
					CheckInDate = r.CheckInDate,
					CheckOutDate = r.CheckOutDate,
				})
				.FirstOrDefaultAsync();

			return dto ?? new CheckOutDTO();
		}
        public async Task<string> GenerateReservationNumberAsync(int branchId)
        {
            var branchCode = await _context.Branches
                .Where(b => b.Id == branchId)
                .Select(b => b.Code)
                .FirstOrDefaultAsync();

            var currentYear = DateTime.UtcNow.Year;

            // Get reservation numbers for this branch and year (only lightweight data)
            var reservationNumbers = await _context.Reservations
                .Where(r => r.BranchId == branchId &&
                            r.CreatedDate.HasValue &&
                            r.CreatedDate.Value.Year == currentYear)
                .Select(r => r.ReservationNumber)
                .Where(rn => rn.StartsWith($"{branchCode}-{currentYear}-"))
                .ToListAsync(); // <-- bring to memory here

            // Now safely parse sequence numbers in memory
            var lastNumber = reservationNumbers
                .Select(rn => int.TryParse(rn.Split('-').Last(), out var seq) ? seq : 0)
                .DefaultIfEmpty(0)
                .Max();

            var nextSequence = lastNumber + 1;

            return $"{branchCode}-{currentYear}-{nextSequence:D6}";
        }


        private DateTime GetEgyptDateTime()
		{
			var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
			var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone).Date;

			return today;
		}

    }


}

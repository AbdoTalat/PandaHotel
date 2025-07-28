using HotelApp.Application.DTOs.Reservation;
using HotelApp.Domain;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.ReservationSourceService
{
    public class ReservationSourceService : IReservationSourceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationSourceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetAllReservationSourceDTO>> GetAllReservationSourcesAsync()
        {
            var reservationSources = await _unitOfWork.Repository<ReservationSource>()
                .GetAllAsDtoAsync<GetAllReservationSourceDTO>(SkipBranchFilter:true);

            return reservationSources;
        }

    }
}

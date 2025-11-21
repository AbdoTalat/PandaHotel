using AutoMapper;
using HotelApp.Application.DTOs.Payment;
using HotelApp.Application.DTOs.RateCalculation;
using HotelApp.Application.Interfaces;
using HotelApp.Application.Services.RateCalculationService;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRateCalculationService _rateCalculationService;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, IRateCalculationService rateCalculationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _rateCalculationService = rateCalculationService;
        }
        
        public async Task<IEnumerable<GetPaymentDTO>> GetPaymentsByReservationIdAsync(int reservationId)
        {
            var payments = await _unitOfWork.PaymentRepository
                .GetAllAsDtoAsync<GetPaymentDTO>(p => p.ReservationId == reservationId);

            return payments;
        }
        public async Task<PaymentDTO> GetPaymentToEditByIdAsync(int paymentId)
        {
            var payment = await _unitOfWork.PaymentRepository
                .GetByIdAsDtoAsync<PaymentDTO>(paymentId);

            return payment ?? new PaymentDTO();
        }
        public async Task<ServiceResponse<PaymentDTO>> AddPaymentAsync(PaymentDTO dto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var payment = _mapper.Map<Payment>(dto);
                await _unitOfWork.PaymentRepository.AddNewAsync(payment);
                await _unitOfWork.CommitAsync();

                var recalculation = await ReservationRecalculationAsync(dto.ReservationId);
                if (!recalculation.Success)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ServiceResponse<PaymentDTO>.ResponseFailure(recalculation.Message);
                }

                await _unitOfWork.CommitTransactionAsync();

                return ServiceResponse<PaymentDTO>.ResponseSuccess("Payment added successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ServiceResponse<PaymentDTO>.ResponseFailure(ex.Message);
            }
        }
        public async Task<ServiceResponse<PaymentDTO>> EditPaymentByIdAsync(PaymentDTO dto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(dto.Id);
                if (payment == null)
                    return ServiceResponse<PaymentDTO>.ResponseFailure("Payment not found.");

                _mapper.Map(dto, payment);
                _unitOfWork.PaymentRepository.Update(payment);
                await _unitOfWork.CommitAsync();

                var recalculation = await ReservationRecalculationAsync(payment.ReservationId);
                if (!recalculation.Success)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ServiceResponse<PaymentDTO>.ResponseFailure(recalculation.Message);
                }

                await _unitOfWork.CommitTransactionAsync();
                return ServiceResponse<PaymentDTO>.ResponseSuccess("Payment updated successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ServiceResponse<PaymentDTO>.ResponseFailure(ex.Message);
            }
        }
        public async Task<ServiceResponse<object>> DeletePaymentByIdAsync(int paymentId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId);
                if (payment == null)
                    return ServiceResponse<object>.ResponseFailure("Payment not found.");

                _unitOfWork.PaymentRepository.Delete(payment);
                await _unitOfWork.CommitAsync();

                var recalculation = await ReservationRecalculationAsync(payment.ReservationId);
                if (!recalculation.Success)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ServiceResponse<object>.ResponseFailure(recalculation.Message);
                }

                await _unitOfWork.CommitTransactionAsync();
                return ServiceResponse<object>.ResponseSuccess("Payment deleted successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ServiceResponse<object>.ResponseFailure(ex.Message);
            }
        }


        #region Helper Methods
        private async Task<ServiceResponse<object>> ReservationRecalculationAsync(int reservationId)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(reservationId);
            if (reservation == null)
                return ServiceResponse<object>.ResponseFailure("Reservation not found.");

            var request = new GetRateCalculationDTORequest
            {
                ReservationId = reservation.Id,
                RateId = reservation.RateId ?? 0,
                CheckIn = reservation.CheckInDate,
                CheckOut = reservation.CheckOutDate,
                RoomTypeQuantities = await _unitOfWork.ReservationRoomTypeRepository
                    .GetAllAsDtoAsync<RoomTypeQuantityDTO>(x => x.ReservationId == reservation.Id)
            };

            var result = await _rateCalculationService.GetRateCalculation(request);

            if (!result.Success || result.Data == null)
                return ServiceResponse<object>.ResponseFailure("Failed to recalculate reservation totals.");

            reservation.TotalPayments = result.Data.TotalPayments;
            reservation.Balance = result.Data.Balance;

            _unitOfWork.ReservationRepository.Update(reservation);
            await _unitOfWork.CommitAsync();

            return ServiceResponse<object>.ResponseSuccess();
        }

        #endregion
    }
}

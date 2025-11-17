using AutoMapper;
using HotelApp.Application.DTOs.Payment;
using HotelApp.Application.Interfaces;
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

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<GetPaymentDTO>> GetPaymentsByReservationIdAsync(int reservationId)
        {
            var payments = await _unitOfWork.PaymentRepository
                .GetAllAsDtoAsync<GetPaymentDTO>(p => p.ReservationId == reservationId);

            return payments;
        }
        public async Task<ServiceResponse<PaymentDTO>> AddPaymentAsync(PaymentDTO dto)
        {
            try
            {
                var payment = _mapper.Map<Payment>(dto);

                await _unitOfWork.PaymentRepository.AddNewAsync(payment);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<PaymentDTO>.ResponseSuccess("Payment added successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<PaymentDTO>.ResponseFailure(ex.Message);
            }
        }

        public async Task<PaymentDTO> GetPaymentToEditByIdAsync(int paymentId)
        {
            var payment = await _unitOfWork.PaymentRepository
                .GetByIdAsDtoAsync<PaymentDTO>(paymentId);

            return payment;
        }

        public async Task<ServiceResponse<PaymentDTO>> EditPaymentByIdAsyc(PaymentDTO dto)
        {                
            var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(dto.Id);    
            if (payment == null)
            {
                return ServiceResponse<PaymentDTO>.ResponseFailure("Payment not found.");
            }

            try
            {
                _mapper.Map(dto, payment);

                _unitOfWork.PaymentRepository.Update(payment);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<PaymentDTO>.ResponseSuccess("Payment updated successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<PaymentDTO>.ResponseFailure(ex.Message);
            }
        }

        public async Task<ServiceResponse<object>> DeletePaymentByIdAsync(int paymentId)
        {
            var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                return ServiceResponse<object>.ResponseFailure("Payment not found.");
            }
            try
            {
                _unitOfWork.PaymentRepository.Delete(payment);
                await _unitOfWork.CommitAsync();

                return ServiceResponse<object>.ResponseSuccess("Payment deleted successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<object>.ResponseFailure(ex.Message);
            }
        }
    }
}

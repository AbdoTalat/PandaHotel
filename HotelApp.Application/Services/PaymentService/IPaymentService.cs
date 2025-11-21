using HotelApp.Application.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<IEnumerable<GetPaymentDTO>> GetPaymentsByReservationIdAsync(int reservationId);
        Task<ServiceResponse<PaymentDTO>> AddPaymentAsync(PaymentDTO dto);
        Task<PaymentDTO> GetPaymentToEditByIdAsync(int paymentId);
        Task<ServiceResponse<PaymentDTO>> EditPaymentByIdAsync(PaymentDTO dto);
        Task<ServiceResponse<object>> DeletePaymentByIdAsync(int paymentId);
    }
}

using HotelApp.Application.DTOs.Payment;
using HotelApp.Application.DTOs.Rooms;
using HotelApp.Application.Services.MasterDataService;
using HotelApp.Application.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelApp.UI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IMasterDataService _masterDataService;

        public PaymentController(IPaymentService paymentService, IMasterDataService masterDataService)
        {
            _paymentService = paymentService;
            _masterDataService = masterDataService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPayments(int reservationId)
        {
            var payments = await _paymentService.GetPaymentsByReservationIdAsync(reservationId);

            return PartialView("_PaymentsTablePartial", payments);
        }

        [HttpGet]
        [Authorize(Policy = "Payment.Add")]
        public async Task<IActionResult> AddPayment(int reservationId, int guestId)
        {
            var model = new PaymentDTO
            {
                ReservationId = reservationId,
                GuestId = guestId,
                PaymentDate = DateTime.Now
            };
            await LoadPaymentDropdownsAsync(model);
            return PartialView("_AddPayment", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Payment.Add")]
        public async Task<IActionResult> AddPayment(PaymentDTO model)
        {
            if (!ModelState.IsValid)
            {
                await LoadPaymentDropdownsAsync(model);
                return PartialView("_AddPayment", model);
            }

            var result = await _paymentService.AddPaymentAsync(model);

            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpGet]
        [Authorize(Policy = "Payment.Edit")]
        public async Task<IActionResult> EditPayment(int Id)
        {
            var model = await _paymentService.GetPaymentToEditByIdAsync(Id);
            await LoadPaymentDropdownsAsync(model);
            return PartialView("_EditPayment", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Payment.Edit")]
        public async Task<IActionResult> EditPayment(PaymentDTO model)
        {
            if (!ModelState.IsValid)
            {
                await LoadPaymentDropdownsAsync(model);
                return PartialView("_EditPayment", model);
            }
            var result = await _paymentService.EditPaymentByIdAsyc(model);
            return Json(new { success = result.Success, message = result.Message });
        }


        [HttpDelete]
        [Authorize(Policy = "Payment.Delete")]
        public async Task<IActionResult> DeletePayment(int Id)
        {
            var result = await _paymentService.DeletePaymentByIdAsync(Id);
            return Json(new { success = result.Success, message = result.Message });
        }


        #region Helper Methods
        private async Task LoadPaymentDropdownsAsync(PaymentDTO model)
        {
            model.TransactionTypes = await _masterDataService.GetTransactionTypesAsync();
            model.PaymentMethods = await _masterDataService.GetPaymentMethodsAsync();
        }
        #endregion
    }
}

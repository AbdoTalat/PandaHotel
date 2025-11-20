using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.MasterDataService
{
    public interface IMasterDataService
    {
        Task<IEnumerable<SelectListItem>> GetTransactionTypesAsync();
        Task<IEnumerable<SelectListItem>> GetPaymentMethodsAsync();
        Task<IEnumerable<SelectListItem>> GetCalculationTypesAsync();
    }
}

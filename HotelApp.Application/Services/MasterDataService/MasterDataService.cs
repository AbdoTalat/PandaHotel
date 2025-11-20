using HotelApp.Application.DTOs;
using HotelApp.Application.Interfaces;
using HotelApp.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.MasterDataService
{
    public class MasterDataService : IMasterDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MasterDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SelectListItem>> GetTransactionTypesAsync()
        {
            var data = await _unitOfWork.MasterDataItemRepository
                .GetAllAsDtoAsync<SelectListItem>(tt => tt.MasterDataTypeId == (int)MasterDataTypeEnum.TransactionType);

            return data;
        }

        public async Task<IEnumerable<SelectListItem>> GetPaymentMethodsAsync()
        {
            var data = await _unitOfWork.MasterDataItemRepository
                .GetAllAsDtoAsync<SelectListItem>(pm => pm.MasterDataTypeId == (int)MasterDataTypeEnum.PaymentMethod);
            return data;
        }

        public async Task<IEnumerable<SelectListItem>> GetCalculationTypesAsync()
        {
            var data = await _unitOfWork.MasterDataItemRepository
                .GetAllAsDtoAsync<SelectListItem>(ct => ct.MasterDataTypeId == (int)MasterDataTypeEnum.CalculationType);
            return data;
        }
    }
}

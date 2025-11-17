using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Domain.Enums
{
    public class MasterDataItemEnums
    {
        public enum TransactionType
        {
            Payment = 1,
            Refund = 2
        }
        public enum PaymentMethod
        {
            Cash = 1,
            CreditCard = 2,
            DebitCard = 3,
            BankTransfer = 4,
            Wallet = 5, 
            Cheque = 6 
        }
    }
}

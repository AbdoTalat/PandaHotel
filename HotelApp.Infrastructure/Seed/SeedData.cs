using HotelApp.Domain.Common;
using HotelApp.Domain.Entities;
using HotelApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HotelApp.Domain.Enums.MasterDataItemEnums;

namespace HotelApp.Infrastructure.Seed
{
    public static class SeedData
    {
        public static void SeedRoomStatuses(this ModelBuilder builder)
        {
            builder.Entity<RoomStatus>().HasData(
                new RoomStatus { Id = 1, Name = "Available", Code = RoomStatusEnum.Available, IsSystem = true, Description = "Room is ready to be booked", BranchId = 2, Color = "#20BF7E" },
                new RoomStatus { Id = 2, Name = "Reserved", Code = RoomStatusEnum.Reserved, IsSystem = true, Description = "Room is reserved by a guest", BranchId = 2, Color = "#20BF7E" },
                new RoomStatus { Id = 3, Name = "Occupied", Code = RoomStatusEnum.Occupied, IsSystem = true, Description = "Room is currently occupied", BranchId = 2, Color = "#20BF7E" },
                new RoomStatus { Id = 4, Name = "Maintenance", Code = RoomStatusEnum.Maintenance, IsSystem = true, Description = "Room is under maintenance", BranchId = 2, Color = "#20BF7E" },
                new RoomStatus { Id = 5, Name = "Cleaning", Code = RoomStatusEnum.Cleaning, IsSystem = true, Description = "Room is being cleaned", BranchId = 2, Color = "#20BF7E" }
            );
        }

        public static void SeedReservationSources(this ModelBuilder builder)
        {
            builder.Entity<ReservationSource>().HasData(
                new ReservationSource { Id = 1, Name = "Walk In", IsActive = true },
                new ReservationSource { Id = 2, Name = "Hotel website", IsActive = true },
                new ReservationSource { Id = 3, Name = "Admin panel", IsActive = true },
                new ReservationSource { Id = 4, Name = "Government", IsActive = true },
                new ReservationSource { Id = 5, Name = "Mobile App", IsActive = true }
            );
        }
        public static void SeedDropDownTypesAndItems(this ModelBuilder builder)
        {
            builder.Entity<MasterDataType>().HasData(
                new MasterDataType { Id = (int)MasterDataTypeEnum.TransactionType, Name = MasterDataTypeEnum.TransactionType.ToString() },
                new MasterDataType { Id = (int)MasterDataTypeEnum.PaymentMethod, Name = MasterDataTypeEnum.PaymentMethod.ToString() }
            );

            builder.Entity<MasterDataItem>().HasData(
                new MasterDataItem {Id = 1, Value = (int)TransactionType.Payment, MasterDataTypeId = (int)MasterDataTypeEnum.TransactionType, Name = TransactionType.Payment.ToString(), IsActive = true },
                new MasterDataItem {Id = 2, Value = (int)TransactionType.Refund, MasterDataTypeId = (int)MasterDataTypeEnum.TransactionType, Name = TransactionType.Refund.ToString(), IsActive = true },

                new MasterDataItem { Id = 3, Value = (int)PaymentMethod.Cash, MasterDataTypeId = (int)MasterDataTypeEnum.PaymentMethod, Name = PaymentMethod.Cash.ToString(), IsActive = true },
                new MasterDataItem { Id = 4, Value = (int)PaymentMethod.CreditCard, MasterDataTypeId = (int)MasterDataTypeEnum.PaymentMethod, Name = PaymentMethod.CreditCard.ToString(), IsActive = true },
                new MasterDataItem { Id = 5, Value = (int)PaymentMethod.DebitCard, MasterDataTypeId = (int)MasterDataTypeEnum.PaymentMethod, Name = PaymentMethod.DebitCard.ToString(), IsActive = true },
                new MasterDataItem { Id = 6, Value = (int)PaymentMethod.BankTransfer, MasterDataTypeId = (int)MasterDataTypeEnum.PaymentMethod, Name = PaymentMethod.BankTransfer.ToString(), IsActive = true },
                new MasterDataItem { Id = 7, Value = (int)PaymentMethod.Wallet, MasterDataTypeId = (int)MasterDataTypeEnum.PaymentMethod, Name = PaymentMethod.Wallet.ToString(), IsActive = true },
                new MasterDataItem { Id = 8, Value = (int)PaymentMethod.Cheque, MasterDataTypeId = (int)MasterDataTypeEnum.PaymentMethod, Name = PaymentMethod.Cheque.ToString(), IsActive = true }

            );
        }
    }
}

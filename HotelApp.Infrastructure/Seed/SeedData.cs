using HotelApp.Domain.Entities;
using HotelApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}

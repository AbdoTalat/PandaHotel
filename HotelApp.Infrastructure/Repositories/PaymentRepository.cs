using AutoMapper;
using HotelApp.Application.Interfaces;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Domain.Entities;
using HotelApp.Infrastructure.DbContext;
using HotelApp.Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context, IConfigurationProvider mappConfig)
            : base(context, mappConfig)
        {

        }
    }
}

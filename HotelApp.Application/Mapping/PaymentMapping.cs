using AutoMapper;
using HotelApp.Application.DTOs.Payment;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Mapping
{
    public class PaymentMapping : Profile
    {
        public PaymentMapping()
        {
            CreateMap<Payment, GetPaymentDTO>()
               // .ForMember(dest => dest.GuestName, opt => opt.MapFrom(src => src.Guest.FullName))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.Name))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType.Name));

            CreateMap<PaymentDTO, Payment>();
            CreateMap<Payment, PaymentDTO>();
        }
    }
}

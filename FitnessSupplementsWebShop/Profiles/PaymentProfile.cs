using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessSupplementsWebShop.Entities;
using FitnessSupplementsWebShop.Models;

namespace FitnessSupplementsWebShop.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentEntity, PaymentDto>();
            CreateMap<PaymentDto, PaymentEntity>();
            CreateMap<PaymentEntity, PaymentUpdateDto>();
            CreateMap<PaymentUpdateDto, PaymentEntity>();

        }
    }
}
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessSupplementsWebShop.Entities;
using FitnessSupplementsWebShop.Models;

namespace FitnessSupplementsWebShop.Profiles
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            CreateMap<OrdersEntity, OrdersDto>();
            CreateMap<OrdersDto, OrdersEntity>();
            CreateMap<OrdersEntity, OrdersDto>();
            CreateMap<OrdersDto, OrdersEntity>();
            CreateMap<OrdersResponse, OrdersDto>();
            CreateMap<OrdersDto, OrdersResponse>();

        }
    }
}
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessSupplementsWebShop.Entities;
using FitnessSupplementsWebShop.Models;

namespace FitnessSupplementsWebShop.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductEntity, ProductDto>();
            CreateMap<ProductDto, ProductEntity>();
            CreateMap<ProductEntity, ProductDto>();
            CreateMap<ProductDto, ProductEntity>();

        }
    }
}
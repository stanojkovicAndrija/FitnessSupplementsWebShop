using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessSupplementsWebShop.Entities;
using FitnessSupplementsWebShop.Models;

namespace FitnessSupplementsWebShop.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryEntity, CategoryDto>();
            CreateMap<CategoryDto, CategoryEntity>();
            CreateMap<CategoryEntity, CategoryDto>();
            CreateMap<CategoryDto, CategoryEntity>();

        }
    }
}
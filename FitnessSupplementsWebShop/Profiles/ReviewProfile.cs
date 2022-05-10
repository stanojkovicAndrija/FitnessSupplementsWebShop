using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessSupplementsWebShop.Entities;
using FitnessSupplementsWebShop.Models;

namespace FitnessSupplementsWebShop.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<ReviewEntity, ReviewDto>();
            CreateMap<ReviewDto, ReviewEntity>();
            CreateMap<ReviewEntity, ReviewUpdateDto>();
            CreateMap<ReviewUpdateDto, ReviewEntity>();

        }
    }
}
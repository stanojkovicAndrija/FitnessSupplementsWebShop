using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessSupplementsWebShop.Entities;
using FitnessSupplementsWebShop.Models;

namespace FitnessSupplementsWebShop.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<UsersEntity, UsersDto>();
            CreateMap<UsersDto, UsersEntity>();
            CreateMap<UsersEntity, UsersDto>();
            CreateMap<UsersDto, UsersEntity>();
        }
    }
}
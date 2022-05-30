using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessSupplementsWebShop.Entities;
using FitnessSupplementsWebShop.Models;

namespace FitnessSupplementsWebShop.Profiles
{
    public class ManufacturerProfile : Profile
    {
        public ManufacturerProfile()
        {
            CreateMap<ManufacturerEntity, ManufacturerDto>();
            CreateMap<ManufacturerDto, ManufacturerEntity>();
            CreateMap<ManufacturerEntity, ManufacturerDto>();
            CreateMap<ManufacturerDto, ManufacturerEntity>();

        }
    }
}
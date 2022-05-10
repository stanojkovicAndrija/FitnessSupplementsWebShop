using AutoMapper;
using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly FitnessSupplementsWebShopContext context;
        private readonly IMapper mapper;

        public ManufacturerRepository(FitnessSupplementsWebShopContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public ManufacturerEntity CreateManufacturer(ManufacturerEntity manufacturer)
        {
            context.Manufacturer.Add(manufacturer);
            return manufacturer;
        }

        public void DeleteManufacturer(int manufacturerID)
        {
            context.Manufacturer.Remove(context.Manufacturer.FirstOrDefault(r => r.ManufacturerID == manufacturerID));
        }

        public ManufacturerEntity GetManufacturerByID(int manufacturerID)
        {
            return context.Manufacturer.FirstOrDefault(r => r.ManufacturerID == manufacturerID);
        }

        public List<ManufacturerEntity> GetManufacturer()
        {
            return (from r in context.Manufacturer select r).ToList();
        }

        public void UpdateManufacturer(ManufacturerEntity manufacturer)
        {
            throw new NotImplementedException();
        }
    }
}
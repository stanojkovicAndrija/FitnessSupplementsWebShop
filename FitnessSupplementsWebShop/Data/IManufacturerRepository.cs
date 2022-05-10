using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public interface IManufacturerRepository
    {
        List<ManufacturerEntity> GetManufacturer();

        ManufacturerEntity GetManufacturerByID(int manufacturerID);

        ManufacturerEntity CreateManufacturer(ManufacturerEntity manufacturer);

        void UpdateManufacturer(ManufacturerEntity manufacturer);

        void DeleteManufacturer(int manufacturerID);

        bool SaveChanges();

    }
}

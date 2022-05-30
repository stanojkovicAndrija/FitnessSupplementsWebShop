using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Models
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryID { get; set; }
        public CategoryDto Category { get; set; }
        public int ManufacturerID { get; set; }
        public ManufacturerDto Manufacturer { get; set; }

    }
}
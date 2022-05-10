using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Entities
{
    public class ProductEntity
    {

            [Key]
            public int ProductID { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public int CategoryID { get; set; }
            public CategoryEntity Category { get; set; }
            public int ManufacturerID { get; set; }
            public ManufacturerEntity Manufacturer { get; set; }


    }
}


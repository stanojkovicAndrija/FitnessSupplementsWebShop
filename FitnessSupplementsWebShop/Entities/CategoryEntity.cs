using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Entities
{
    public class CategoryEntity
    {

            [Key]
            public int CategoryID { get; set; }
            public string Name { get; set; }

    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Entities
{
    public class ManufacturerEntity
    {

            [Key]
            public int ManufacturerID { get; set; }
            public string Name { get; set; }
 

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Models
{
    public class ProductResponse
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}

using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Models
{
    public class OrderitemDto
    {
        public int ProductID { get; set; }
        public ProductDto Product { get; set; }
        public int OrderID { get; set; }
        public OrdersDto Order { get; set; }
    }
}

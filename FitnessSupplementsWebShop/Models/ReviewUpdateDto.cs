using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Models
{
    public class ReviewUpdateDto
    {
        public int ReviewID { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int UserID { get; set; }
        public UsersDto User { get; set; }
        public int ProductID { get; set; }
        public ProductDto Product { get; set; }
    }
}

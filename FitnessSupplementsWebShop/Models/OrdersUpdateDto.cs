using System;
using System.Collections.Generic;
using System.Linq;
using FitnessSupplementsWebShop.Entities;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Models
{
    public class OrdersUpdateDto
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public UsersDto User { get; set; }
        public int PaymentID { get; set; }
        public PaymentDto Payment { get; set; }

        public string OrderAddress { get; set; }
        public string City { get; set; }
        public int NumberOfProducts { get; set; }
        public decimal Total { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Entities
{
    public class OrdersEntity
    {

            [Key]
            public int OrderID { get; set; }
            public int UserID { get; set; }
            public UsersEntity User { get; set; }
            public int PaymentID { get; set; }
            public PaymentEntity Payment { get; set; }

            public string OrderAddress { get; set; }
            public string City { get; set; }
            public int NumberOfProducts { get; set; }
            public decimal Total { get; set; }

    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace FitnessSupplementsWebShop.Entities
{
    public class OrderitemEntity
    {

            [Key]
            public int OrderItemID { get; set; }
            public int ProductID { get; set; }
            public ProductEntity Product { get; set; }
            public int OrderID { get; set; }
            public OrdersEntity Order { get; set; }
            public int Quantity { get; set; }

    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Entities
{
    public class ReviewEntity
    {

            [Key]
            public int ReviewID { get; set; }
            public string Comment { get; set; }
            public int Rating { get; set; }
            public int UserID { get; set; }
            public UsersEntity User { get; set; }
            public int ProductID { get; set; }
            public ProductEntity Product { get; set; }

    }
}


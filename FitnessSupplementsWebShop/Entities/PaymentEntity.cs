using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Entities
{
    public class PaymentEntity
    {

            [Key]
            public int PaymentID { get; set; }
            public string PaymentMethod { get; set; }


    }
}


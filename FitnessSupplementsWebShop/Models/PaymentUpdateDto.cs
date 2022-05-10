using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Models
{
    public class PaymentUpdateDto
    {
        public int PaymentID { get; set; }
        public string PaymentMethod { get; set; }

    }
}

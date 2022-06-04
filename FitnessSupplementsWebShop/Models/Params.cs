using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Models
{
    public class Params
    {
        public int? CategoryID { get; set; }
        public int? ManufacturerID { get; set; }
        public int PageIndex { get; set; }
        public string Sort { get; set; }
        
        public string Search { get; set; }

    }
}

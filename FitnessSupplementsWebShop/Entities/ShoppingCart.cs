using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            
        }
        public ShoppingCart(string id){
            ShoppingCartID = id;
        }
        public string ShoppingCartID { get; set; }
        public List<CartItem> Items { get; set; } = new List<
        CartItem>();
    }
}

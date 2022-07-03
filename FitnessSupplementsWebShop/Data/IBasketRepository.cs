using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetShoppingCartAsync(string cartID);
        Task<ShoppingCart> UpdateShoppingCartAsync(ShoppingCart cart);
        Task<bool> DeleteShoppingCartsAsync(string cartID);
    }
}

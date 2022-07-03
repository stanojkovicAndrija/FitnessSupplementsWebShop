using FitnessSupplementsWebShop.Data;
using FitnessSupplementsWebShop.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCartByID(string cartID)
        {
            var cart = await shoppingCartRepository.GetShoppingCartAsync(cartID);
            return Ok(cart ?? new ShoppingCart(cartID));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateShoppingCartAsync(ShoppingCart cart)
        {

            var updatedBasket = await shoppingCartRepository.UpdateShoppingCartAsync(cart);

            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteShoppingCartAsync(string cartID)
        {
             await shoppingCartRepository.DeleteShoppingCartsAsync(cartID);
        }
    }
}

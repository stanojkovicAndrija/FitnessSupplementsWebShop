using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Text.Json;

namespace FitnessSupplementsWebShop.Data
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IDatabase _database;
        public ShoppingCartRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteShoppingCartsAsync(string cartID)
        {
            return await _database.KeyDeleteAsync(cartID);
        }

        public async Task<ShoppingCart> GetShoppingCartAsync(string cartID)
        {
            var data = await _database.StringGetAsync(cartID);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data);
        }

        public async Task<ShoppingCart> UpdateShoppingCartAsync(ShoppingCart cart)
        {
            var created = await _database.StringSetAsync(cart.ShoppingCartID, JsonSerializer.Serialize(cart),TimeSpan.FromDays(30));

            if (!created) return null;

            return await GetShoppingCartAsync(cart.ShoppingCartID);
        }
    }
}

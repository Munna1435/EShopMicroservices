using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository
        (IBasketRepository repository, IDistributedCache cache) 
        : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasket(string usernName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(usernName, cancellationToken);
            if (!string.IsNullOrWhiteSpace(cachedBasket)) {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
            }

            var basket = await repository.GetBasket(usernName, cancellationToken);
            await cache.SetStringAsync(usernName, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await repository.StoreBasket(basket, cancellationToken);

            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<bool> DeleteBasket(string usernName, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(usernName, cancellationToken);

            await cache.RemoveAsync(usernName, cancellationToken);

            return true;
        }
    }
}

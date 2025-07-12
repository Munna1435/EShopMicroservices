using Basket.API.Exceptions;
using Basket.API.Models;
using Marten;

namespace Basket.API.Data
{
    public class BasketRepository(IDocumentSession session) : IBasketRepository
    {
        //private readonly IDocumentSession _session;

        //public BasketRepository(IDocumentSession session)
        //{
        //    _session = session;
        //}

        public async Task<ShoppingCart> GetBasket(string usernName, CancellationToken cancellationToken = default)
        {
            var basket = await session.LoadAsync<ShoppingCart>(usernName, cancellationToken);
            return basket ?? throw new BasketNotFoundException(usernName);
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            session.Store(basket); //it performs upsert operation
            await session.SaveChangesAsync(cancellationToken);
            return basket;
        }

        public async Task<bool> DeleteBasket(string usernName, CancellationToken cancellationToken = default)
        {
            session.Delete<ShoppingCart>(usernName);
            await session.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

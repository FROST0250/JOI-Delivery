using JoiDelivery.Application.Interfaces;
using JoiDelivery.Infrastructure.Seed;
using JoiDelivery.Models;

namespace JoiDelivery.Infrastructure.Repositories;

public sealed class InMemoryProductRepository(InMemoryDataStore dataStore) : IProductRepository
{
    public GroceryProduct? GetByProductAndOutlet(string productId, string outletId) =>
        dataStore.GroceryProducts.FirstOrDefault(product =>
            product.Id == productId && product.BelongsToOutlet(outletId));
}

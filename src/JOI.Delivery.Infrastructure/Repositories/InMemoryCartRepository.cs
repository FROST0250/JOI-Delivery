using JoiDelivery.Application.Interfaces;
using JoiDelivery.Infrastructure.Seed;
using JoiDelivery.Models;

namespace JoiDelivery.Infrastructure.Repositories;

public sealed class InMemoryCartRepository(InMemoryDataStore dataStore) : ICartRepository
{
    public Cart? GetByUserId(string userId) =>
        dataStore.CartForUsers.GetValueOrDefault(userId);

    public void Save(Cart cart)
    {
        ArgumentNullException.ThrowIfNull(cart);

        if (cart.User is not null)
        {
            dataStore.CartForUsers[cart.User.Id] = cart;
            return;
        }

        var existingUserId = dataStore.CartForUsers.FirstOrDefault(pair => pair.Value.Id == cart.Id).Key;
        if (!string.IsNullOrWhiteSpace(existingUserId))
        {
            dataStore.CartForUsers[existingUserId] = cart;
        }
    }
}

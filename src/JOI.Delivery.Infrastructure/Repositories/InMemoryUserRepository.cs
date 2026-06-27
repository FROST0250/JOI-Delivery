using JoiDelivery.Application.Interfaces;
using JoiDelivery.Infrastructure.Seed;
using JoiDelivery.Models;

namespace JoiDelivery.Infrastructure.Repositories;

public sealed class InMemoryUserRepository(InMemoryDataStore dataStore) : IUserRepository
{
    public User? GetById(string userId) =>
        dataStore.Users.FirstOrDefault(user => user.Id == userId);
}

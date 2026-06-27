using Xunit;
using FluentAssertions;
using JoiDelivery.Infrastructure.Repositories;
using JoiDelivery.Infrastructure.Seed;
using JoiDelivery.Models;

namespace JOI.Delivery.Infrastructure.Tests.Repositories;

public class InMemoryCartRepositoryTests
{
    [Fact]
    public void GetByUserId_WhenCartExists_ReturnsCart()
    {
        var repository = new InMemoryCartRepository(new InMemoryDataStore());

        var result = repository.GetByUserId("user101");

        result.Should().NotBeNull();
        result!.Id.Should().Be("cart101");
    }

    [Fact]
    public void Save_WhenCartHasUser_StoresCartByUserId()
    {
        var dataStore = new InMemoryDataStore();
        var repository = new InMemoryCartRepository(dataStore);
        var cart = new Cart { Id = "cart999", User = new User { Id = "user999" } };

        repository.Save(cart);

        repository.GetByUserId("user999").Should().Be(cart);
    }

    [Fact]
    public void Save_WhenCartHasNoUserButExistingCartId_ReplacesExistingCart()
    {
        var dataStore = new InMemoryDataStore();
        var repository = new InMemoryCartRepository(dataStore);
        var cart = new Cart { Id = "cart101" };

        repository.Save(cart);

        repository.GetByUserId("user101").Should().Be(cart);
    }

    [Fact]
    public void Save_WhenCartIsNull_ThrowsArgumentNullException()
    {
        var repository = new InMemoryCartRepository(new InMemoryDataStore());

        var act = () => repository.Save(null!);

        act.Should().Throw<ArgumentNullException>();
    }
}

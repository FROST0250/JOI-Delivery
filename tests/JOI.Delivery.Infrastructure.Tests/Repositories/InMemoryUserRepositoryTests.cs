using Xunit;
using FluentAssertions;
using JoiDelivery.Infrastructure.Repositories;
using JoiDelivery.Infrastructure.Seed;

namespace JOI.Delivery.Infrastructure.Tests.Repositories;

public class InMemoryUserRepositoryTests
{
    [Fact]
    public void GetById_WhenUserExists_ReturnsUser()
    {
        var repository = new InMemoryUserRepository(new InMemoryDataStore());

        var result = repository.GetById("user101");

        result.Should().NotBeNull();
        result!.FirstName.Should().Be("John");
    }

    [Fact]
    public void GetById_WhenUserDoesNotExist_ReturnsNull()
    {
        var repository = new InMemoryUserRepository(new InMemoryDataStore());

        var result = repository.GetById("unknown");

        result.Should().BeNull();
    }
}

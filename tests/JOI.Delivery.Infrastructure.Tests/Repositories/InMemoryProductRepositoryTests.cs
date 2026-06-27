using Xunit;
using FluentAssertions;
using JoiDelivery.Infrastructure.Repositories;
using JoiDelivery.Infrastructure.Seed;

namespace JOI.Delivery.Infrastructure.Tests.Repositories;

public class InMemoryProductRepositoryTests
{
    [Fact]
    public void GetByProductAndOutlet_WhenProductBelongsToOutlet_ReturnsProduct()
    {
        var repository = new InMemoryProductRepository(new InMemoryDataStore());

        var result = repository.GetByProductAndOutlet("product101", "store101");

        result.Should().NotBeNull();
        result!.Id.Should().Be("product101");
    }

    [Fact]
    public void GetByProductAndOutlet_WhenOutletDoesNotMatch_ReturnsNull()
    {
        var repository = new InMemoryProductRepository(new InMemoryDataStore());

        var result = repository.GetByProductAndOutlet("product101", "store999");

        result.Should().BeNull();
    }
}

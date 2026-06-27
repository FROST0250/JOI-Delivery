using Xunit;
using FluentAssertions;
using JoiDelivery.Models;

namespace JOI.Delivery.Domain.Tests.Models;

public class GroceryProductTests
{
    [Fact]
    public void BelongsToOutlet_WhenProductStoreMatchesOutlet_ReturnsTrue()
    {
        var product = new GroceryProduct
        {
            Id = "product101",
            Store = new GroceryStore { Id = "store101" }
        };

        var result = product.BelongsToOutlet("store101");

        result.Should().BeTrue();
    }

    [Fact]
    public void BelongsToOutlet_WhenProductStoreDoesNotMatchOutlet_ReturnsFalse()
    {
        var product = new GroceryProduct
        {
            Id = "product101",
            Store = new GroceryStore { Id = "store101" }
        };

        var result = product.BelongsToOutlet("store999");

        result.Should().BeFalse();
    }

    [Fact]
    public void BelongsToOutlet_WhenStoreIsMissing_ReturnsFalse()
    {
        var product = new GroceryProduct { Id = "product101" };

        var result = product.BelongsToOutlet("store101");

        result.Should().BeFalse();
    }
}

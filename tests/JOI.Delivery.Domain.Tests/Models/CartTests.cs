using Xunit;
using FluentAssertions;
using JoiDelivery.Models;

namespace JOI.Delivery.Domain.Tests.Models;

public class CartTests
{
    [Fact]
    public void AddProduct_WhenCartHasExistingProducts_AppendsProduct()
    {
        var existingProduct = new GroceryProduct { Id = "product101" };
        var newProduct = new GroceryProduct { Id = "product102" };
        var cart = new Cart { Id = "cart101", Products = [existingProduct] };

        cart.AddProduct(newProduct);

        cart.Products.Should().ContainInOrder(existingProduct, newProduct);
    }

    [Fact]
    public void AddProduct_WhenProductsCollectionIsMissing_InitializesCollection()
    {
        var product = new GroceryProduct { Id = "product101" };
        var cart = new Cart { Id = "cart101", Products = null! };

        cart.AddProduct(product);

        cart.Products.Should().ContainSingle().Which.Should().Be(product);
    }

    [Fact]
    public void AddProduct_WhenProductIsNull_ThrowsArgumentNullException()
    {
        var cart = new Cart { Id = "cart101" };

        var act = () => cart.AddProduct(null!);

        act.Should().Throw<ArgumentNullException>();
    }
}

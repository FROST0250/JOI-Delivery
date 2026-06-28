using Xunit;
using FluentAssertions;
using JoiDelivery.Models;

namespace JOI.Delivery.Domain.Tests.Models;

public class CartTests
{
    [Fact]
    public void AddProduct_WhenCartHasExistingProducts_AppendsProduct()
    {
        var outletId = Guid.NewGuid().ToString();
        var existingProduct = new GroceryProduct { Id = "product101" , Store = new GroceryStore { Id = outletId} };
        var newProduct = new GroceryProduct { Id = "product102", Store = new GroceryStore { Id = outletId} };
        var cart = new Cart { Id = "cart101"};
        cart.AddProduct(existingProduct);

        cart.AddProduct(newProduct);

        cart.Products.Should().ContainInOrder(existingProduct, newProduct);
    }

    [Fact]
    public void AddProduct_WhenProductsCollectionIsMissing_InitializesCollection()
    {
        var product = new GroceryProduct { Id = "product101" };
        var cart = new Cart { Id = "cart101" };

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

    [Fact]
    public void AddProduct_ShouldThrowDomainValidationException_WhenProductIsFromDifferentOutlet()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var outletA = new Outlet { Id = Guid.NewGuid().ToString(), Name = "Outlet A" };
        var outletB = new Outlet { Id = Guid.NewGuid().ToString(), Name = "Outlet B" };

        var cart = new Cart { Id = userId.ToString()};

        var firstProduct = new GroceryProduct { Id = Guid.NewGuid().ToString(), Name = "Bread", SellingPrice = 5.00f, Store = new GroceryStore { Id = outletA.Id, Name = outletA.Name } };
        var secondProduct = new GroceryProduct { Id = Guid.NewGuid().ToString(), Name = "Milk", SellingPrice = 2.49f, Store = new GroceryStore { Id = outletB.Id, Name = outletB.Name } };

        // Act & Assert
        // 1. Adding the first product should succeed and set the baseline outlet
        cart.AddProduct(firstProduct);

        // 2. Adding the second product from a different outlet should throw the exception
        Action act = () => cart.AddProduct(secondProduct);

        act.Should().Throw<DomainValidationException>()
            .WithMessage("Cannot mix items from different outlets in a single cart.");
    }
}

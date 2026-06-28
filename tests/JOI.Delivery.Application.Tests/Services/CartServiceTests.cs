using Xunit;
using FluentAssertions;
using JoiDelivery.Application.Interfaces;
using JoiDelivery.Dtos;
using JoiDelivery.Exceptions;
using JoiDelivery.Models;
using JoiDelivery.Services;
using Moq;

namespace JOI.Delivery.Application.Tests.Services;

public class CartServiceTests
{
    private readonly Mock<ICartRepository> cartRepository = new();
    private readonly Mock<IProductService> productService = new();
    private readonly Mock<IUserService> userService = new();

    [Fact]
    public void AddProductToCartForUser_WhenRequestIsValid_AddsProductAndSavesCart()
    {
        var request = new ProductRequest { UserId = "user101", ProductId = "product101", OutletId = "store101" };
        var user = new User { Id = request.UserId };
        var cart = new Cart { Id = "cart101", User = user };
        var product = new GroceryProduct { Id = request.ProductId, SellingPrice = 7.5f };
        userService.Setup(service => service.FetchUserById(request.UserId)).Returns(user);
        cartRepository.Setup(repository => repository.GetByUserId(request.UserId)).Returns(cart);
        productService.Setup(service => service.GetProduct(request.ProductId, request.OutletId)).Returns(product);
        var service = CreateService();

        var result = service.AddProductToCartForUser(request);

        result.Cart.Should().Be(cart);
        result.Product.Should().Be(product);
        result.SellingPrice.Should().Be(product.SellingPrice);
        cart.Products.Should().Contain(product);
        cartRepository.Verify(repository => repository.Save(cart), Times.Once);
    }

    [Fact]
    public void AddProductToCartForUser_WhenRequestIsNull_ThrowsArgumentNullException()
    {
        var service = CreateService();

        var act = () => service.AddProductToCartForUser(null!);

        act.Should().Throw<ArgumentNullException>();
        cartRepository.Verify(repository => repository.Save(It.IsAny<Cart>()), Times.Never);
    }

    [Theory]
    [InlineData("", "product101", "store101")]
    [InlineData("   ", "product101", "store101")]
    [InlineData("user101", "", "store101")]
    [InlineData("user101", "   ", "store101")]
    [InlineData("user101", "product101", "")]
    [InlineData("user101", "product101", "   ")]
    public void AddProductToCartForUser_WhenRequiredInputIsMissing_ThrowsValidationException(string userId, string productId, string outletId)
    {
        var request = new ProductRequest { UserId = userId, ProductId = productId, OutletId = outletId };
        var service = CreateService();

        var act = () => service.AddProductToCartForUser(request);

        act.Should().Throw<ApplicationValidationException>();
        cartRepository.Verify(repository => repository.Save(It.IsAny<Cart>()), Times.Never);
    }

    [Fact]
    public void AddProductToCartForUser_WhenUserDoesNotExist_ThrowsNotFoundException()
    {
        var request = CreateAddProductRequest();
        userService.Setup(service => service.FetchUserById(request.UserId)).Returns((User?)null);
        var service = CreateService();

        var act = () => service.AddProductToCartForUser(request);

        act.Should().Throw<ApplicationNotFoundException>().WithMessage("*User 'user101'*");
        cartRepository.Verify(repository => repository.Save(It.IsAny<Cart>()), Times.Never);
    }

    [Fact]
    public void AddProductToCartForUser_WhenCartDoesNotExist_ThrowsNotFoundException()
    {
        var request = CreateAddProductRequest();
        var user = new User { Id = request.UserId };
        userService.Setup(service => service.FetchUserById(request.UserId)).Returns(user);
        cartRepository.Setup(repository => repository.GetByUserId(request.UserId)).Returns((Cart?)null);
        var service = CreateService();

        var act = () => service.AddProductToCartForUser(request);

        act.Should().Throw<ApplicationNotFoundException>().WithMessage("*Cart for user 'user101'*");
        cartRepository.Verify(repository => repository.Save(It.IsAny<Cart>()), Times.Never);
    }

    [Fact]
    public void AddProductToCartForUser_WhenProductDoesNotExist_ThrowsNotFoundException()
    {
        var request = CreateAddProductRequest();
        var user = new User { Id = request.UserId };
        var cart = new Cart { Id = "cart101", User = user };
        userService.Setup(service => service.FetchUserById(request.UserId)).Returns(user);
        cartRepository.Setup(repository => repository.GetByUserId(request.UserId)).Returns(cart);
        productService.Setup(service => service.GetProduct(request.ProductId, request.OutletId)).Returns((GroceryProduct?)null);
        var service = CreateService();

        var act = () => service.AddProductToCartForUser(request);

        act.Should().Throw<ApplicationNotFoundException>().WithMessage("*Product 'product101'*outlet 'store101'*");
        cart.Products.Should().BeEmpty();
        cartRepository.Verify(repository => repository.Save(It.IsAny<Cart>()), Times.Never);
    }

    [Fact]
    public void GetCartForUser_WhenUserIdIsValid_ReturnsCartFromRepository()
    {
        var cart = new Cart { Id = "cart101" };
        cartRepository.Setup(repository => repository.GetByUserId("user101")).Returns(cart);
        var service = CreateService();

        var result = service.GetCartForUser("user101");

        result.Should().Be(cart);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void GetCartForUser_WhenUserIdIsMissing_ThrowsValidationException(string userId)
    {
        var service = CreateService();

        var act = () => service.GetCartForUser(userId);

        act.Should().Throw<ApplicationValidationException>().WithMessage("User id is required.");
    }

    [Fact]
    public void RemoveProductFromCartForUser_WhenProductIsInCart_RemovesProductAndSavesCart()
    {
        var request = CreateAddProductRequest();
        var user = new User { Id = request.UserId };
        var cart = new Cart { Id = "cart101", User = user };
        var product = new GroceryProduct { Id = request.ProductId, SellingPrice = 7.5f };
        cart.AddProduct(product);
        userService.Setup(service => service.FetchUserById(request.UserId)).Returns(user);
        cartRepository.Setup(repository => repository.GetByUserId(request.UserId)).Returns(cart);
        productService.Setup(service => service.GetProduct(request.ProductId, request.OutletId)).Returns(product);
        var service = CreateService();

        bool result = service.RemoveProductFromCartForUser(request);

        cart.Products.Should().NotContain(product);
        cartRepository.Verify(repository => repository.Save(cart), Times.Once);
    }

    [Fact]
    public void ClearCartForUser_WhenCartExists_RemovesAllProductsAndSavesCart()
    {
        var request = CreateAddProductRequest();
        var user = new User { Id = request.UserId };
        var cart = new Cart { Id = "cart101", User = user };
        var product1 = new GroceryProduct { Id = "product101", SellingPrice = 7.5f };
        var product2 = new GroceryProduct { Id = "product102", SellingPrice = 5.0f };

        cart.AddProduct(product1);
        cart.AddProduct(product2);

        userService.Setup(service => service.FetchUserById(request.UserId)).Returns(user);
        productService.Setup(service => service.GetProduct(product1.Id, request.OutletId)).Returns(product1);
        productService.Setup(service => service.GetProduct(product2.Id, request.OutletId)).Returns(product2);
        cartRepository.Setup(repository => repository.GetByUserId(request.UserId)).Returns(cart);
        var service = CreateService();

        //Act
        var clearRequest = service.ClearCart(request.UserId);

        //Assert
        cart.Products.Should().BeEmpty();
        cartRepository.Verify(repository => repository.Save(cart), Times.Once);
        clearRequest.Should().Be(true);
    }

    private static ProductRequest CreateAddProductRequest() => new() { UserId = "user101", ProductId = "product101", OutletId = "store101" };

    private CartService CreateService() => new(cartRepository.Object, productService.Object, userService.Object);
}

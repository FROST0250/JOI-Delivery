using Xunit;
using FluentAssertions;
using JoiDelivery.Application.Interfaces;
using JoiDelivery.Models;
using JoiDelivery.Services;
using Moq;

namespace JOI.Delivery.Application.Tests.Services;

public class ProductServiceTests
{
    [Fact]
    public void GetProduct_DelegatesLookupToRepository()
    {
        var product = new GroceryProduct { Id = "product101" };
        var repository = new Mock<IProductRepository>();
        repository.Setup(repo => repo.GetByProductAndOutlet("product101", "store101")).Returns(product);
        var service = new ProductService(repository.Object);

        var result = service.GetProduct("product101", "store101");

        result.Should().Be(product);
    }
}

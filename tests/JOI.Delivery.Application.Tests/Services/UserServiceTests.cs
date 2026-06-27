using Xunit;
using FluentAssertions;
using JoiDelivery.Application.Interfaces;
using JoiDelivery.Models;
using JoiDelivery.Services;
using Moq;

namespace JOI.Delivery.Application.Tests.Services;

public class UserServiceTests
{
    [Fact]
    public void FetchUserById_DelegatesLookupToRepository()
    {
        var user = new User { Id = "user101" };
        var repository = new Mock<IUserRepository>();
        repository.Setup(repo => repo.GetById("user101")).Returns(user);
        var service = new UserService(repository.Object);

        var result = service.FetchUserById("user101");

        result.Should().Be(user);
    }
}

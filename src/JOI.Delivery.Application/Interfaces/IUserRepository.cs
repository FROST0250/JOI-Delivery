using JoiDelivery.Models;

namespace JoiDelivery.Application.Interfaces;

public interface IUserRepository
{
    User? GetById(string userId);
}

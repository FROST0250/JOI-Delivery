using JoiDelivery.Models;

namespace JoiDelivery.Services;

public interface IUserService
{
    User? FetchUserById(string userId);
}

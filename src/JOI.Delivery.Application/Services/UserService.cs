using JoiDelivery.Application.Interfaces;
using JoiDelivery.Models;

namespace JoiDelivery.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public User? FetchUserById(string userId) => userRepository.GetById(userId);
}

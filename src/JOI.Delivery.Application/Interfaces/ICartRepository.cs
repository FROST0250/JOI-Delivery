using JoiDelivery.Models;

namespace JoiDelivery.Application.Interfaces;

public interface ICartRepository
{
    Cart? GetByUserId(string userId);
    void Save(Cart cart);
}

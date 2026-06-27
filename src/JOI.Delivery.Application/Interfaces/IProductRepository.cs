using JoiDelivery.Models;

namespace JoiDelivery.Application.Interfaces;

public interface IProductRepository
{
    GroceryProduct? GetByProductAndOutlet(string productId, string outletId);
}

using JoiDelivery.Models;

namespace JoiDelivery.Services;

public interface IProductService
{
    GroceryProduct? GetProduct(string productId, string outletId);
}

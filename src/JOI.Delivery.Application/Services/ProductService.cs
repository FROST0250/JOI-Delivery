using JoiDelivery.Application.Interfaces;
using JoiDelivery.Models;

namespace JoiDelivery.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public GroceryProduct? GetProduct(string productId, string outletId) =>
        productRepository.GetByProductAndOutlet(productId, outletId);
}

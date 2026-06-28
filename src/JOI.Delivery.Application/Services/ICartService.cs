using JoiDelivery.Dtos;
using JoiDelivery.Models;

namespace JoiDelivery.Services;

public interface ICartService
{
    CartProductInfo AddProductToCartForUser(ProductRequest addProductRequest);
    bool ClearCart(string userId);
    Cart? GetCartForUser(string userId);
    bool RemoveProductFromCartForUser(ProductRequest removeProductRequest);
}

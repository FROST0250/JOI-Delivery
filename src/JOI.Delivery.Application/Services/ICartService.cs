using JoiDelivery.Dtos;
using JoiDelivery.Models;

namespace JoiDelivery.Services;

public interface ICartService
{
    CartProductInfo AddProductToCartForUser(AddProductRequest addProductRequest);
    Cart? GetCartForUser(string userId);
}

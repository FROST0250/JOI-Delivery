using JoiDelivery.Application.Interfaces;
using JoiDelivery.Dtos;
using JoiDelivery.Exceptions;
using JoiDelivery.Models;

namespace JoiDelivery.Services;

public class CartService(
    ICartRepository cartRepository,
    IProductService productService,
    IUserService userService) : ICartService
{
    public CartProductInfo AddProductToCartForUser(ProductRequest addProductRequest)
    {
        Validate(addProductRequest);

        var user = userService.FetchUserById(addProductRequest.UserId)
            ?? throw new ApplicationNotFoundException($"User '{addProductRequest.UserId}' was not found.");

        var cart = cartRepository.GetByUserId(user.Id)
            ?? throw new ApplicationNotFoundException($"Cart for user '{user.Id}' was not found.");

        var product = productService.GetProduct(addProductRequest.ProductId, addProductRequest.OutletId)
            ?? throw new ApplicationNotFoundException(
                $"Product '{addProductRequest.ProductId}' was not found for outlet '{addProductRequest.OutletId}'.");

        cart.AddProduct(product);
        cartRepository.Save(cart);

        return new CartProductInfo(cart, product, product.SellingPrice);
    }

    public Cart? GetCartForUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ApplicationValidationException("User id is required.");
        }

        return cartRepository.GetByUserId(userId);
    }

    private static void Validate(ProductRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            throw new ApplicationValidationException("User id is required.");
        }

        if (string.IsNullOrWhiteSpace(request.ProductId))
        {
            throw new ApplicationValidationException("Product id is required.");
        }

        if (string.IsNullOrWhiteSpace(request.OutletId))
        {
            throw new ApplicationValidationException("Outlet id is required.");
        }
    }

    public bool RemoveProductFromCartForUser(ProductRequest removeProductRequest)
    {
        var cart = cartRepository.GetByUserId(removeProductRequest.UserId) ??
            throw new ApplicationNotFoundException($"Cart for user '{removeProductRequest.UserId}' was not found.");

        var product = productService.GetProduct(removeProductRequest.ProductId, removeProductRequest.OutletId) ??
            throw new ApplicationNotFoundException($"Product '{removeProductRequest.ProductId}' was not found for outlet '{removeProductRequest.OutletId}'.");
        if (!cart.Products.Contains(product))
            throw new ApplicationValidationException($"Product '{removeProductRequest.ProductId}' is not in the cart for user '{removeProductRequest.UserId}'.");

        cart.Products.Remove(product);
        cartRepository.Save(cart);
        return true;

    }

    public bool ClearCart(string userId)
    {
        var cart = cartRepository.GetByUserId(userId);
        if (cart != null)
        {
            cart.Products.Clear();
            cartRepository.Save(cart);
            return true;

        }
        else
        {
            throw new ApplicationNotFoundException($"Cart for user '{userId}' was not found.");
        }
    }
}
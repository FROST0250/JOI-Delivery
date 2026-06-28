using JoiDelivery.Dtos;
using JoiDelivery.Models;
using JoiDelivery.Services;
using Microsoft.AspNetCore.Mvc;

namespace JoiDelivery.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpPost("product")]
    public ActionResult<CartProductInfo> AddProductToCart([FromBody] ProductRequest addProductRequest)
    {
        var result = cartService.AddProductToCartForUser(addProductRequest);

        return Ok(result);
    }

    [HttpGet("view")]
    public ActionResult<Cart> ViewCart([FromQuery(Name = "userId")] string userId)
    {
        var cart = cartService.GetCartForUser(userId);

        return Ok(cart);
    }

    [HttpDelete("product")]
    public ActionResult<CartProductInfo> RemoveproductFromCart([FromBody] ProductRequest removeProductRequest)
    {
        var result = cartService.RemoveProductFromCartForUser(removeProductRequest);

        return Ok();
    }

    [HttpPost("clear")]
    public ActionResult ClearCart([FromBody] string userId)
    {
        var result = cartService.ClearCart(userId);
        return Ok();
    }

}

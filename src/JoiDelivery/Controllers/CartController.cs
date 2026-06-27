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
    public ActionResult<CartProductInfo> AddProductToCart([FromBody] AddProductRequest addProductRequest)
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
}

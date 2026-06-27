namespace JoiDelivery.Dtos;

public class AddProductRequest
{
    public string OutletId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}

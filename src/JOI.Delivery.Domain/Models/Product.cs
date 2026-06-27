namespace JoiDelivery.Models;

public abstract class Product
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public float MaxRecommendedPrice { get; set; }
}

namespace JoiDelivery.Models;

public class Cart
{
    public string Id { get; set; } = string.Empty;
    public Outlet? Outlet { get; set; }
    public List<GroceryProduct> Products { get; set; } = [];
    public User? User { get; set; }

    public void AddProduct(GroceryProduct product)
    {
        ArgumentNullException.ThrowIfNull(product);

        Products ??= [];
        Products.Add(product);
    }
}

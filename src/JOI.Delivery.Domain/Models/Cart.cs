namespace JoiDelivery.Models;

public class Cart
{
    public string Id { get; set; } = string.Empty;
    public Outlet? Outlet { get; private set; }
    private List<Product> _products = [];

    public IReadOnlyCollection<Product> Products => _products;
    public User? User { get; set; }

    public void AddProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        _products ??= [];

        var productOutlet = GetOutlet(product);
        if (Outlet == null)
        {
            Outlet = productOutlet;
        }
        else if (productOutlet.Id != Outlet.Id)
        {
            throw new DomainValidationException("Cannot mix items from different outlets in a single cart.");
        }

        _products.Add(product);
    }

    public void RemoveProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        _products.Remove(product);
        if (_products.Count == 0)
        {
            Outlet = null;
        }
    }

    public void ClearProducts()
    {
        _products.Clear();
        Outlet = null; 
    }

    private static Outlet GetOutlet(Product product) => product switch
    {
        GroceryProduct groceryProduct => new Outlet { Id = groceryProduct.Store?.Id ?? string.Empty, Name = groceryProduct.Store?.Name ?? string.Empty },
        FoodProduct foodProduct => new Outlet { Id = foodProduct.Restaurant?.Id ?? string.Empty, Name = foodProduct.Restaurant?.Name ?? string.Empty },
        _ => throw new DomainValidationException($"Unsupported product type: {product.GetType().Name}.")
    };
}

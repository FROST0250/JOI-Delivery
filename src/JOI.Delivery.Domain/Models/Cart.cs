namespace JoiDelivery.Models;

public class Cart
{
    public string Id { get; set; } = string.Empty;
    public Outlet? Outlet { get; private set; }
    private List<GroceryProduct> _products = [];

    public IReadOnlyCollection<GroceryProduct> Products => _products;
    public User? User { get; set; }

    public void AddProduct(GroceryProduct product)
    {
        ArgumentNullException.ThrowIfNull(product);

        _products ??= [];
        if (Outlet == null)
        {
            Outlet = new Outlet { Id = product.Store?.Id ?? string.Empty, Name = product.Store?.Name ?? string.Empty };
        }
        else
        {
            if (!product.BelongsToOutlet(Outlet.Id))
            {
                throw new DomainValidationException("Cannot mix items from different outlets in a single cart.");
            }
        }
        _products.Add(product);
    }

    public void RemoveProduct(GroceryProduct product)
    {
        ArgumentNullException.ThrowIfNull(product);

        _products.Remove(product);
        if(_products.Count == 0)
        {
            Outlet = null;
        }
    }

    public void ClearProducts()
    {
        _products.Clear();
        Outlet = null;
    }
}

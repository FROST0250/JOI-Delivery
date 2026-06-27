namespace JoiDelivery.Models;

public class GroceryStore : Outlet
{
    public HashSet<GroceryProduct> Inventory { get; set; } = [];
}

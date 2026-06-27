namespace JoiDelivery.Models;

public class GroceryProduct : Product
{
    public float SellingPrice { get; set; }
    public float Weight { get; set; }
    public int ExpiryDate { get; set; }
    public int Threshold { get; set; }
    public int AvailableStock { get; set; }
    public float Discount { get; set; }
    public GroceryStore? Store { get; set; }

    public bool BelongsToOutlet(string outletId) =>
        Store is not null && Store.Id == outletId;
}

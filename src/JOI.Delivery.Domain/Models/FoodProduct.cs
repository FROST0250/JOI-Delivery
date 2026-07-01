namespace JoiDelivery.Models;

public class FoodProduct : Product
{
    public float SellingPrice { get; set; }
    public bool isAvailable { get; set; }
    public Restaurant? Restaurant { get; set; }

    public bool belongsToRestaurant(string restaurantId) =>
        Restaurant is not null && Restaurant.Id == restaurantId;
}

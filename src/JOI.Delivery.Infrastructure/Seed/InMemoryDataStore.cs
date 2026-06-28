using JoiDelivery.Models;

namespace JoiDelivery.Infrastructure.Seed;

public sealed class InMemoryDataStore
{
    public InMemoryDataStore()
    {
        Store101 = CreateStore("Fresh Picks", "store101");
        Store102 = CreateStore("Natural Choice", "store102");
        User101 = CreateUser("user101", "John", "Doe");
        User102 = CreateUser("user102", "Joe", "Dan");

        CartForUsers = new Dictionary<string, Cart>
        {
            { "user101", CreateCartForUser("user101", "John", "Doe", "cart101") },
            { "user102", CreateCartForUser("user102", "Rachel", "Zane", "cart102") }
        };

        GroceryProducts =
        [
            CreateGroceryProduct("Wheat Bread", "product101", Store101),
            CreateGroceryProduct("Spinach", "product102", Store101),
            CreateGroceryProduct("Crackers", "product103", Store101)
        ];

        Users = [User101,User102];
    }

    public Dictionary<string, Cart> CartForUsers { get; }
    public GroceryStore Store101 { get; }
    public GroceryStore Store102 { get; }
    public User User101 { get; }
    public User User102 { get; }
    public List<GroceryProduct> GroceryProducts { get; }
    public List<User> Users { get; }

    private Cart CreateCartForUser(string userId, string firstName, string lastName, string cartId)
    {
        return new Cart
        {
            Id = cartId,
            User = CreateUser(userId, firstName, lastName),
        };
    }

    private static GroceryStore CreateStore(string outletName, string storeId)
    {
        return new GroceryStore
        {
            Name = outletName,
            Id = storeId
        };
    }

    private static User CreateUser(string userId, string firstName, string lastName)
    {
        return new User
        {
            Id = userId,
            FirstName = firstName,
            LastName = lastName,
            Email = $"{firstName}.{lastName}@gmail.com",
            PhoneNumber = "100000000"
        };
    }

    private static GroceryProduct CreateGroceryProduct(string productName, string productId, GroceryStore store)
    {
        return new GroceryProduct
        {
            Name = productName,
            Id = productId,
            MaxRecommendedPrice = 10.5f,
            Weight = 500.00f,
            Store = store,
            Threshold = 10,
            AvailableStock = 30
        };
    }
}

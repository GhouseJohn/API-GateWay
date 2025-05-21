namespace Basket.API.Model
{
    public class ShoppingCart
    {
        public string? Id { get; set; } = default;  // Required by RavenDB
        public string UserName { get; set; } = default!;
        public List<ShoppingCartItem> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        //Required for Mapping
        public ShoppingCart()
        {
        }
    }
}

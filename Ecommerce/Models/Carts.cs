namespace Ecommerce.Models
{
    public class Carts
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public AspNetUser User { get; set; }

        public ICollection <CartItem> CartItems { get; set; } = new List<CartItem>();
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Carts Cart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}

namespace Ecommerce.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AspNetUser User { get; set; }
        public decimal TotalAmount { get; set; } = 0;
        public Payments Payment { get; set; }
    }
}

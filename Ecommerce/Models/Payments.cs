namespace Ecommerce.Models
{
    public class Payments
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Orders Order { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string UserId { get; set; }
        public AspNetUser User { get; set; }
    }
}

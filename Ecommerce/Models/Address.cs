namespace Ecommerce.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string UserId { get; set; }
        public AspNetUser User { get; set; }
    }
}

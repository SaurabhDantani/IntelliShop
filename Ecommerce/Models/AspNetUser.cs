using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models
{
    public class AspNetUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Address Address { get; set; }
        public ICollection<Carts> Carts { get; set; } = new List<Carts>();
        public ICollection<Orders> Orders { get; set; } = new List<Orders>();
        public ICollection<Payments> Payments { get; set; } = new List<Payments>();
    }
}

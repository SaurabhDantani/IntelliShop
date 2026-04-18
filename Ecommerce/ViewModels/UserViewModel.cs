namespace Ecommerce.ViewModels
{
    public class UserViewModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class UserRegisterViewModel
    {
        public required string Name { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}

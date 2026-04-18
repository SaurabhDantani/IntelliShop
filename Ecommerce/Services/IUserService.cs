using Ecommerce.Models;

namespace Ecommerce.Services
{
    public interface IUserService
    {
        Task<AspNetUser?> GetByIdAsync(string userId);
        Task<IEnumerable<AspNetUser>> GetAllAsync();
        Task<bool> DeactivateUserAsync(string userId);
    }
}

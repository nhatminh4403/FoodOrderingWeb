using FoodOrderingWeb.Models;

namespace FoodOrderingWeb.Repository.Framework
{
    public interface Interface_CartRepository
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart> GetByIdAsync(int id);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(int id);
    }
}

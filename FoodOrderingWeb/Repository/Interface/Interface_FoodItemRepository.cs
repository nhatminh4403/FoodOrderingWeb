using FoodOrderingWeb.Models;
namespace FoodOrderingWeb.Repository.Interface
{
    public interface Interface_FoodItemRepository
    {
        Task<IEnumerable<FoodItem>> GetAllAsync();
        Task<FoodItem> GetByIdAsync(int id);
        Task AddAsync(FoodItem foodItem);
        Task UpdateAsync(FoodItem foodItem);
        Task DeleteAsync(int id);
    }
}

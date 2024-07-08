using FoodOrderingWeb.Models;

namespace FoodOrderingWeb.Repository.Interface
{
    public interface Interface_RestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant> FindByIdAsync(int id);
        Task AddAsync(Restaurant restaurant);
        Task DeleteAsync(int id);
        Task UpdateAsync(Restaurant restaurant);
    }
}

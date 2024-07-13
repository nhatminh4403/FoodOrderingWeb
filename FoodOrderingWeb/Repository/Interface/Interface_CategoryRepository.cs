using FoodOrderingWeb.Models;

namespace FoodOrderingWeb.Repository.Interface
{
    public interface Interface_CategoryRepository
    {
        Task <IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync (Category category);
        Task DeleteAsync (int id);
        Task UpdateAsync (Category category);
    }
}

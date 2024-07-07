using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Models;
using FoodOrderingWeb.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingWeb.Repository.Framework
{
    public class EF_Category : Interface_CategoryRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;
        public EF_Category(ApplicationDatabaseContext context)
        {
            _databaseContext = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _databaseContext.Categories.Include(p=>p.FoodItems).ToListAsync();
        }
        public async Task<Category> FindByIdAsync(int id)
        {
            return await _databaseContext.Categories.Include(p => p.FoodItems).FirstAsync(p=>p.Id == id);
        }
        public async Task AddAsync(Category category)
        {
            _databaseContext.Categories.Add(category);
            await _databaseContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var category = await _databaseContext.Categories.FindAsync(id);
            if(category != null)
            {
                _databaseContext.Categories.Remove(category);
                await _databaseContext.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(Category category)
        {
            _databaseContext.Categories.Update(category);
            await _databaseContext.SaveChangesAsync();
        }
    }
}

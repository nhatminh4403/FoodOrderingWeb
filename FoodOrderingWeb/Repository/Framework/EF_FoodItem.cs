using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Models;
using FoodOrderingWeb.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingWeb.Repository.Framework
{
    public class EF_FoodItem : Interface_FoodItemRepository
    {
        private readonly ApplicationDatabaseContext _context;
        public EF_FoodItem(ApplicationDatabaseContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<FoodItem>> GetAllAsync()
        {
            return await _context.FoodItems.Include(p=>p.Category).ToListAsync();
        }
        public async Task<FoodItem> GetByIdAsync(int id)
        {
            return await _context.FoodItems.Include(p => p.Category).FirstAsync(p=>p.FoodId == id);
        }
        public async Task AddAsync(FoodItem foodItem)
        {
            _context.FoodItems.Add(foodItem);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(FoodItem foodItem)
        {
            _context.FoodItems.Update(foodItem);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var food = await _context.FoodItems.FindAsync(id);
            if (food != null)
            {
                _context.FoodItems.Remove(food);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FoodItem>> GetFoodByCategory(int categoryId)
        {
            return await _context.FoodItems.Include(p=> p.Category).Where(p=>p.CategoryId == categoryId).ToListAsync();
        }
        public async Task<IEnumerable<FoodItem>> GetFoodsByRestaurant(int restaurantId)
        {
            return await _context.FoodItems.Where(p=>p.Equals(restaurantId)).ToListAsync();   
        }
    }
}

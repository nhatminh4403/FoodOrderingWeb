using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Models;
using FoodOrderingWeb.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingWeb.Repository.Framework
{
    public class EF_Restaurant : Interface_RestaurantRepository
    {
        private readonly ApplicationDatabaseContext _context;
        public EF_Restaurant(ApplicationDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await _context.Restaurants.Include(r=>r.FoodItems).ToListAsync();
        }
        public async Task<Restaurant> FindByIdAsync(int id) { return await _context.Restaurants.Include(r => r.FoodItems).FirstAsync(r=>r.RestaurantId==id); }
        public async Task AddAsync(Restaurant restaurant)
        { 
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var restaurant= await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
            }

        }
        public async Task UpdateAsync(Restaurant restaurant) 
        { 
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }
    }
}

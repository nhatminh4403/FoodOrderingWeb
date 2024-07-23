using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Models;
using FoodOrderingWeb.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingWeb.Repository.EF
{
    public class EF_Cart : Interface_CartRepository
    {
        private readonly ApplicationDatabaseContext _context;
        public EF_Cart(ApplicationDatabaseContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _context.Carts
/*                .Include(p=>p.Orders)
*/                .ToListAsync();
        }
        public async Task<Cart> GetByIdAsync(int id)
        {
            return await _context.Carts.SingleAsync(x => x.CartID == id);
        }
        public async Task AddAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
            return;
        }
    }
}

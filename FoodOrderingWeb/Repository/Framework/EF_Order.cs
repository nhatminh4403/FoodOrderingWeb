using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Models;
using FoodOrderingWeb.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingWeb.Repository.Framework
{
    public class EF_Order : Interface_OrderRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;
        public EF_Order(ApplicationDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task<IEnumerable<Order>> GetAllAsync() 
        {
            return await _databaseContext.Orders.Include(o => o.OrderDetails).ToListAsync();
        }
        public async Task<Order> GetByIdAsync(int id) 
        { 
            return await _databaseContext.Orders.Include(o => o.OrderDetails).FirstAsync(o => o.Id == id);
        }
        public async Task AddAsync(Order order)
        {
            _databaseContext.Orders.Add(order);
            await _databaseContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Order order) 
        {
            _databaseContext.Orders.Update(order);
            await _databaseContext.SaveChangesAsync();
       }
        public async Task DeleteAsync(int id) 
        {
            var order = await _databaseContext.Orders.FindAsync(id);
            if (order != null)
            {
                _databaseContext.Orders.Remove(order);
                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}

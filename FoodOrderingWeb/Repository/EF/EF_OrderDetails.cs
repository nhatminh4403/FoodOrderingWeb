using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Models;
using FoodOrderingWeb.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingWeb.Repository.EF
{
    public class EF_OrderDetails : Interface_OrderDetailsRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;
        public EF_OrderDetails(ApplicationDatabaseContext context)
        {
            _databaseContext = context;
        }

        public async Task<OrderDetail> GetOrderDetailsByIdAsync(int id)
        {
            return await _databaseContext.OrderDetails.Include(o=>o.Order).Include(o=>o.FoodItem).FirstAsync(o=>o.OrderId == id);
        }
        public async Task AddAsync(OrderDetail orderDetail)
        {
            _databaseContext.OrderDetails.Add(orderDetail);
            await _databaseContext.SaveChangesAsync();
        }
    }
}

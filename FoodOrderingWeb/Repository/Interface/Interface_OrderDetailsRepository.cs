using FoodOrderingWeb.Models;

namespace FoodOrderingWeb.Repository.Interface
{
    public interface Interface_OrderDetailsRepository
    {
        Task<OrderDetail> GetOrderDetailsByIdAsync(int id);
        Task AddAsync(OrderDetail orderDetail);
    }
}

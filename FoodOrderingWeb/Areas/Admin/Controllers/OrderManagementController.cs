using FoodOrderingWeb.Models;
using FoodOrderingWeb.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Role_Admin)]
    public class OrderManagementController : Controller
    {
        private readonly Interface_OrderRepository _orderRepository;
        private readonly Interface_OrderDetailsRepository _orderDetailsRepository;
        public OrderManagementController(Interface_OrderDetailsRepository orderDetailsRepository,Interface_OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailsRepository = orderDetailsRepository;
        }
        public async  Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllAsync();
            return View(orders);
        }
    }
}

using FoodOrderingWeb.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingWeb.Controllers
{
    public class CartController : Controller
	{

		private readonly Interface_CartRepository _cartRepository;
		private readonly Interface_OrderRepository _orderRepository;
		public CartController(Interface_CartRepository cartRepository, Interface_OrderRepository orderRepository)
		{
			_cartRepository = cartRepository;
			_orderRepository = orderRepository;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Checkout() 
		{
			return View();
		}
	}
}

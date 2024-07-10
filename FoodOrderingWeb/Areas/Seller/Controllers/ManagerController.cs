using FoodOrderingWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingWeb.Areas.Seller.Controllers
{
	[Area("Seller")]
	[Authorize(Roles =Role.Role_Seller)]
	public class ManagerController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

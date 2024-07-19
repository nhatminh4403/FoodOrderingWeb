using FoodOrderingWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Role_Admin)]
    public class ManagerController : Controller
	{
		public IActionResult Index()
		{
            ViewData["CurrentPage"] = "Trang chủ";

            return View();
		}
        public IActionResult Charts() 
        {
            ViewData["CurrentPage"] = "Thống kê";
            return View();
        }
	}
}

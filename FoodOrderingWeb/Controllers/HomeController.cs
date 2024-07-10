using FoodOrderingWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FoodOrderingWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger,UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

		public async Task<IActionResult> Index()
        {
			if (User.Identity.IsAuthenticated)
			{
				var user = await _userManager.GetUserAsync(User);
				if (user != null)
				{
					if (await _userManager.IsInRoleAsync(user, "Admin"))
					{
						return RedirectToAction("Index", "Manager", new { area = "Admin" });
					}
                    else if(await _userManager.IsInRoleAsync(user, "Seller"))
                    {
                        return RedirectToAction("Index", "Manager", new { area = "Seller" });
                    }
				}

			}
			return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

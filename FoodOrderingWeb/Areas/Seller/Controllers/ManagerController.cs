using FoodOrderingWeb.Areas.Seller.ViewModel;
using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Models;
using FoodOrderingWeb.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingWeb.Areas.Seller.Controllers
{
	[Area("Seller")]
	[Authorize(Roles =Role.Role_Seller)]
	public class ManagerController : Controller
	{
		private readonly Interface_RestaurantRepository _restaurantRepository;
		private readonly UserManager<User> _userManager;
		private readonly ApplicationDatabaseContext _databaseContext;
		public ManagerController(Interface_RestaurantRepository restaurantRepository, UserManager<User> userManager, ApplicationDatabaseContext context)
		{
			_databaseContext = context;
			_userManager = userManager;
			_restaurantRepository = restaurantRepository;
		}
		public async  Task<IActionResult> Index()
		{
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var restaurantName = string.Empty;

            if (user != null)
            {
                var restaurant = _databaseContext.Restaurants.FirstOrDefault(r => r.UserId == user.Id);
                restaurantName = restaurant?.RestaurantName ?? string.Empty;
            }

            var model = new RestaurantName
            {
                Name = restaurantName
            };
            return View();
		}
	}
}

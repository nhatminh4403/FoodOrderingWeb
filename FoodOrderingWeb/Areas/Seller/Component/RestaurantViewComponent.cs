using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingWeb.Areas.Seller.Component
{
    public class RestaurantViewComponent : ViewComponent
    {

        private readonly ApplicationDatabaseContext _context;
        private readonly UserManager<User> _userManager;

        public RestaurantViewComponent(ApplicationDatabaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return View("Default", string.Empty);
            }

            var restaurant = _context.Restaurants.FirstOrDefault(r => r.UserId == user.Id);
            var restaurantName = restaurant?.RestaurantName ?? string.Empty;

            return View("Default", restaurantName);
        }
    }
}

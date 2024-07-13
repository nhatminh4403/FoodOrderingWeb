using FoodOrderingWeb.Models;
using FoodOrderingWeb.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodOrderingWeb.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = Role.Role_Seller)]
    public class FoodController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly Interface_FoodItemRepository _foodItemRepository;
        private readonly Interface_CategoryRepository _categoryRepository;
        public FoodController(UserManager<User> userManager, Interface_FoodItemRepository foodItemRepository, Interface_CategoryRepository categoryRepository)
        {
            _userManager = userManager;
            _foodItemRepository = foodItemRepository;
            _categoryRepository = categoryRepository;
        }
        private bool IsLegitInt(string idString)
        {
            if (int.TryParse(idString, out int id))
            {
                // Parsing succeeded, id contains the parsed integer value
                return true;
            }
            else
            {
                // Parsing failed, idString is not a valid integer
                return false;
            }
        }
        // GET: FoodController
        public async Task<IActionResult> Index()
        {
            var restaurantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!IsLegitInt(restaurantId))
            {
                return NotFound("Restaurant not found for the current user.");
            }
            else
            {
                var foodByRestaurant= await _foodItemRepository.GetFoodsByRestaurant(Convert.ToInt32(restaurantId));
                return View(foodByRestaurant);
            }
        }
        private async Task<string?> SaveImage(IFormFile image)
        {

            var savePath = Path.Combine("wwwroot/categoryIcons", image.FileName); //

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/categoryIcons/" + image.FileName;
        }
        // GET: FoodController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: FoodController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FoodController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: FoodController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FoodController/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: FoodController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

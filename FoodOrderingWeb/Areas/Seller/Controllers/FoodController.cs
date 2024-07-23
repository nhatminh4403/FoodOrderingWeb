using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Models;
using FoodOrderingWeb.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodOrderingWeb.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = Role.Role_Seller)]
    public class FoodController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDatabaseContext _context;
        private readonly Interface_FoodItemRepository _foodItemRepository;
        private readonly Interface_CategoryRepository _categoryRepository;
        public FoodController(UserManager<User> userManager, Interface_FoodItemRepository foodItemRepository, ApplicationDatabaseContext context,
            Interface_CategoryRepository categoryRepository)
        {
            _context = context;
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

            var savePath = Path.Combine("wwwroot/foodImages", image.FileName); //

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/foodImages/" + image.FileName;
        }
        // GET: FoodController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var food = await _foodItemRepository.GetByIdAsync(id);
            if (food == null)
                return NotFound();

            return View(food);
        }

        // GET: FoodController/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            ViewBag.Categories = new SelectList(categories,"Id", "Name");
            return View();
        }

        // POST: FoodController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodItem food, IFormFile MainPictureUrl, List<IFormFile> PictureLists)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                // Lấy nhà hàng của người dùng
                var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.UserId == user.Id);
                if (restaurant == null)
                {
                    return NotFound("Restaurant not found for the logged-in user.");
                }

                food.RestaurantId = restaurant.RestaurantId;

                if (MainPictureUrl != null)
                {
                    food.MainPictureUrl = await SaveImage(MainPictureUrl);
                }
                if(PictureLists != null)
                {
                    food.PictureLists= new List<PictureLists>();
                    foreach(var item  in PictureLists)
                    {
                        PictureLists image = new PictureLists
                        {
                            FoodItemId = food.FoodId,
                            Url = await SaveImage(item)
                        };
                    }
                }
                await _foodItemRepository.AddAsync(food);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var categories = await _categoryRepository.GetCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(food);
            }
        }

        // GET: FoodController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var food = await _foodItemRepository.GetByIdAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(food);
        }

        // POST: FoodController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile MainPictureUrl, List<IFormFile> PictureLists, FoodItem food)
        {
            ModelState.Remove("MainPictureUrl");
            if (id != food.FoodId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingFood = await _foodItemRepository.GetByIdAsync(id);
                


                if (MainPictureUrl == null)
                {
                    food.MainPictureUrl = existingFood.MainPictureUrl;
                }
                else
                {
                    existingFood.MainPictureUrl = await SaveImage(MainPictureUrl);
                }
                if (PictureLists != null)
                {
                    food.PictureLists = new List<PictureLists>();
                    foreach (var item in PictureLists)
                    {
                        PictureLists image = new PictureLists
                        {
                            FoodItemId = food.FoodId,
                            Url = await SaveImage(item)
                        };
                    }
                }

                existingFood.FoodName = food.FoodName;
                existingFood.FoodPrice = food.FoodPrice;
                existingFood.FoodDescription = food.FoodDescription;
                existingFood.CategoryId=food.CategoryId;
                existingFood.MainPictureUrl = food.MainPictureUrl;
                await _foodItemRepository.UpdateAsync(existingFood);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var categories = await _categoryRepository.GetCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(food);
            }
        }

        // GET: FoodController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var food = await _foodItemRepository.GetByIdAsync(id);
            if (food == null)
                return NotFound();

            return View(food);
        }

        // POST: FoodController/Delete/5
        [HttpPost,ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _foodItemRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

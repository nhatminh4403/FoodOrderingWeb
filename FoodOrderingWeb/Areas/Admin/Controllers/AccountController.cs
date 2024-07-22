using FoodOrderingWeb.Areas.Admin.ViewModel;
using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =Role.Role_Admin)]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDatabaseContext _context;
        public AccountController(UserManager<User> userManager, ApplicationDatabaseContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: AccountController
        public async Task<IActionResult> Index()
        {
            var restaurants = await _context.Restaurants.Include(r => r.User).ToListAsync();
            IList<User> customers = await _userManager.GetUsersInRoleAsync("Customer");

            var viewModel = new AccountVM
            {
                Restaurants = restaurants.Select(r => new RestaurantAccVM
                {
                    Restaurant = r,
                    LockoutEnd = r.User.LockoutEnd
                }).ToList(),
                Customers = customers.Select(user => new CustomerAccVM
                {
                    User = user,
                    LockoutEnd = user.LockoutEnd
                }).ToList()
            };
            ViewData["CurrentPage"] = "Quản lý tài khoản";
            return View(viewModel);
        }


        public async Task<IActionResult> CustomerAccount()
        {
            IList<User> customer = await _userManager.GetUsersInRoleAsync("Customer");
            var customerAccount = new List<CustomerAccVM>();
            foreach(var user in customer)
            {
                customerAccount.Add(new CustomerAccVM
                {
                    User = user,
                    LockoutEnd = user.LockoutEnd
                });
            }
            return View(customerAccount);
        }
        [HttpPost]
        public async Task<IActionResult> DisableAccount(string userId, int? days)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Kiểm tra giá trị days và thiết lập LockoutEnd tương ứng
            if (days.HasValue)
            {
                if (days.Value == -1)  // Nếu days = -1, vô hiệu hóa vĩnh viễn
                {
                    user.LockoutEnd = DateTimeOffset.MaxValue;
                }
                else if (days.Value == 0)  // Nếu days = 0, không khóa tài khoản
                {
                    user.LockoutEnd = null; // Khóa tài khoản không có thời gian khóa
                }
                else
                {
                    user.LockoutEnd = DateTimeOffset.UtcNow.AddDays(days.Value);  // Khóa tài khoản trong khoảng thời gian days
                }
            }
            else
            {
                user.LockoutEnd = DateTimeOffset.MaxValue;  // Nếu không có giá trị days, vô hiệu hóa vĩnh viễn
            }

            user.LockoutEnabled = true;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

    }
}

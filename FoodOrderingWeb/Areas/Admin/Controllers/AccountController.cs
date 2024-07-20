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
            var userList = await _userManager.Users.ToListAsync();
            ViewData["CurrentPage"] = "Tài khoản";

            var userWithRoles = new List<UserRolesViewModel>();
            foreach (var user in userList) 
            {
                var roles = await _userManager.GetRolesAsync(user);
                userWithRoles.Add(new UserRolesViewModel
                {
                    User=user,
                    Roles=roles
                });
            }

            return View(userWithRoles);
        }

        [HttpPost]
        public async Task<IActionResult> DisableAccount(string userId, int? days)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (days.HasValue)
            {
                if (days.Value == 0)
                {
                    user.LockoutEnd = DateTimeOffset.MaxValue;
                }
                else
                {
                    user.LockoutEnd = DateTimeOffset.UtcNow.AddDays(days.Value);
                }
            }
            else
            {
                user.LockoutEnd = DateTimeOffset.MaxValue;
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

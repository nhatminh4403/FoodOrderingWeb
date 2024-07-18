using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using FoodOrderingWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FoodOrderingWeb.DataAccess;

namespace FoodOrderingWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RestaurantRegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDatabaseContext _databaseContext;
        public RestaurantRegisterModel(
            UserManager<User> userManager, IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,RoleManager<IdentityRole> roleManager,ApplicationDatabaseContext context,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _databaseContext = context;
            _signInManager = signInManager;
            _logger = logger;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }


            [Required(ErrorMessage = "Tên không được để trống")]
            [StringLength(100000, MinimumLength = 5, ErrorMessage = "Tên ít nhất 5 ký tự")]
            public string? RestaurantName { get; set; }
            [Required(ErrorMessage = "Không được bỏ trống")]
            [StringLength(100000, MinimumLength = 5, ErrorMessage = "Địa chỉ ít nhất 5 ký tự")]
            public string? RestaurantAddress { get; set; }
            [Required(ErrorMessage = "Store Phone Number is required")]
            [Phone(ErrorMessage = "Invalid Phone Number")]
            [DataType(DataType.Password)]
            public string? StorePhoneNumber { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!_roleManager.RoleExistsAsync(Role.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Role.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Role.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Role.Role_Seller)).GetAwaiter().GetResult();

            }
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new User { 
                    UserName = Input.Email,
                    Email = Input.Email,
                    FullName="",
                    PhoneNumber="",
                    DefaultAddress=""
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var restaurant = new Restaurant
                    {
                        UserId = user.Id,
                        RestaurantName = Input.RestaurantName,
                        RestaurantAddress = Input.RestaurantAddress,
                        StorePhoneNumber = Input.StorePhoneNumber,
                    };
                    user.FullName = restaurant.RestaurantName;
                    user.PhoneNumber = restaurant.StorePhoneNumber;
                    user.DefaultAddress= restaurant.RestaurantAddress;
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, Role.Role_Seller);
                    _databaseContext.Restaurants.Add(restaurant);
                    await _databaseContext.SaveChangesAsync();

                    
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            if (string.IsNullOrEmpty(Input.Email) || string.IsNullOrEmpty(Input.Password) ||
                string.IsNullOrEmpty(Input.RestaurantName) || string.IsNullOrEmpty(Input.RestaurantAddress) ||
                string.IsNullOrEmpty(Input.StorePhoneNumber))
            {
                ModelState.AddModelError(string.Empty, "All fields are required.");
                return Page();
            }
            return Page();
        }
        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}

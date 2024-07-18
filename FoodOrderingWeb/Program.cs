using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Models;
using Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FoodOrderingWeb.Repository.Interface;
using FoodOrderingWeb.Repository.Framework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDatabaseContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddIdentity<User, IdentityRole>()
 .AddDefaultTokenProviders()
 .AddDefaultUI()
 .AddEntityFrameworkStores<ApplicationDatabaseContext>();
builder.Services.AddRazorPages();

builder.Services.AddScoped<Interface_FoodItemRepository,EF_FoodItem>();
builder.Services.AddScoped<Interface_CategoryRepository, EF_Category>();
builder.Services.AddScoped<Interface_OrderDetailsRepository, EF_OrderDetails>();
builder.Services.AddScoped<Interface_OrderRepository, EF_Order>();
builder.Services.AddScoped<Interface_PictureListRepository, EF_PictureList>();
builder.Services.AddScoped<Interface_CartRepository, EF_Cart>();
builder.Services.AddScoped<Interface_RestaurantRepository, EF_Restaurant>();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = $"/Identity/Account/Login";
    option.LogoutPath = $"/Identity/Account/Logout";
    option.LogoutPath = $"/Identity/Account/AccessDenied";

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); 
app.UseAuthentication(); 
app.UseAuthorization();
app.MapRazorPages();
/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
*/
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "Admin",
		pattern: "{area:exists}/{controller=Manager}/{action=Index}/{id?}"
	);
	endpoints.MapControllerRoute(
		name: "Seller",
		pattern: "{area:exists}/{controller=Manager}/{action=Index}/{id?}"
	);
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}"
	);
});
app.MapRazorPages().RequireAuthorization();
app.Run();

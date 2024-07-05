using FoodOrderingWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingWeb.DataAccess
{
    public class ApplicationDatabaseContextFoodWeb : IdentityDbContext<User>
    {
        public ApplicationDatabaseContextFoodWeb(DbContextOptions<ApplicationDatabaseContextFoodWeb> options) : base(options) { }

        public DbSet<User> Users {  get; set; }
        public DbSet<Role> Roles {  get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

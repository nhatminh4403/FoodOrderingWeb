using FoodOrderingWeb.Models;
namespace FoodOrderingWeb.Areas.Admin.ViewModel
{
    public class UserRolesViewModel
    {
        public User User { get; set; }
        public IList<string> Roles { get; set; }
    }
}

using FoodOrderingWeb.Models;

namespace FoodOrderingWeb.Areas.Admin.ViewModel
{
    public class CustomerAccVM
    {
        public User User { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}

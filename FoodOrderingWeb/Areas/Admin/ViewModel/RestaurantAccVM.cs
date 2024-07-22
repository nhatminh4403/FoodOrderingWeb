using FoodOrderingWeb.Models;

namespace FoodOrderingWeb.Areas.Admin.ViewModel
{
    public class RestaurantAccVM
    {
         public Restaurant Restaurant { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}

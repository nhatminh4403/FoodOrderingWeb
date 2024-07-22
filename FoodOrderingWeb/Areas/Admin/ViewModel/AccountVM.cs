using FoodOrderingWeb.Models;
namespace FoodOrderingWeb.Areas.Admin.ViewModel
{
    public class AccountVM
    {
        public List<RestaurantAccVM> Restaurants { get; set; }
        public List<CustomerAccVM> Customers { get; set; }
    }
}

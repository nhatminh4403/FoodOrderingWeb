using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FoodOrderingWeb.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Không được để trống")]
        [StringLength(50)]
        [DisplayName("Họ và tên")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [DisplayName("Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public  string? PhoneNumber { get; set; }
       
        [DisplayName("Địa chỉ")]
        public string? DefaultAddress { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingWeb.Models
{
    public class Restaurant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int RestaurantId { get; set; }

        [Required]
        public int? UserId { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [Required(ErrorMessage ="Tên không được để trống")]
        [StringLength(100000,MinimumLength =5,ErrorMessage ="Tên ít nhất 5 ký tự")]
        public string? RestaurantName { get; set; }
        [Required(ErrorMessage ="Không được bỏ trống")]
        [StringLength(100000, MinimumLength = 5, ErrorMessage = "Địa chỉ ít nhất 5 ký tự")]
        public string? RestaurantAddress { get; set; }
        [Required(ErrorMessage = "Store Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [DataType(DataType.Password)]
        public string? StorePhoneNumber { get; set; }

        public ICollection<FoodItem>? FoodItems { get; set; }
    }
}

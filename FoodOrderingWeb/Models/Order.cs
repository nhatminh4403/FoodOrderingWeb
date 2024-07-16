using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FoodOrderingWeb.Models.Enum;

namespace FoodOrderingWeb.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "Order Date is required")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        public double TotalAmount { get; set; }

        [Required]
        public string ShippingAddress { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
    }
}

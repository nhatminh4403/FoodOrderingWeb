using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderingWeb.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [Required(ErrorMessage = "Order Date is required")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

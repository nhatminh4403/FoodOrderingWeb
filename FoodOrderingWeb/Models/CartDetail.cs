using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderingWeb.Models
{
    public class CartDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartDetailID { get; set; }

        [Required]
        public int CartID { get; set; }

        [Required]
        public int FoodItemId { get; set; }
        public string? FoodName { get; set; }
        public string? CategoryDescription { get; set; }
        [Required]
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        // Navigation properties
        [ForeignKey(nameof(CartID))]
        public virtual Cart? Cart { get; set; }

        [ForeignKey("FoodItemId")]
        public virtual FoodItem? FoodItem { get; set; }
    }
}

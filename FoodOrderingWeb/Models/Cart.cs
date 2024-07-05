using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderingWeb.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartID { get; set; }

        [Required]
        public int? UserID { get; set; }

        public virtual ICollection<CartDetail>? CartDetails { get; set; }
        public virtual User? User { get; set; }


        [NotMapped]
        public List<CartDetail> Items { get; set; } = new List<CartDetail>();
        public void AddItem(CartDetail item)
        {
            var existingItem = Items.FirstOrDefault(i => i.FoodItemId== item.FoodItemId);
            if (existingItem != null)
            {
                existingItem.Quantity = existingItem.Quantity + item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }
        public void RemoveItem(int foodItemId)
        {
            Items.RemoveAll(i => i.FoodItemId == foodItemId);
        }
        public void IncreaseQuantity(int foodItemId)
        {
            var item = Items.FirstOrDefault(i => i.FoodItemId == foodItemId);
            if (item != null)
            {
                item.Quantity++;
            }
        }
        public void DecreaseQuantity(int foodItemId)
        {
            var item = Items.FirstOrDefault(i => i.FoodItemId == foodItemId);
            if (item != null && item.Quantity > 1)
            {
                item.Quantity--;
            }
        }
    }
}

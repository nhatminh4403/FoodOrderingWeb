using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingWeb.Models
{
    public class PictureLists
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id {  get; set; }
        [Required]
        public string? Url { get; set; }
        [Required]
        public int? FoodItemId { get; set; }
        [ForeignKey(nameof(FoodItemId))]
        public virtual FoodItem? FoodItem { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingWeb.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Tên")]
        public string? Name { get; set; }

        [Required]
        public string? CategoryIcon { get; set; }

        public virtual ICollection<FoodItem>? FoodItems { get; set; }
    }
}

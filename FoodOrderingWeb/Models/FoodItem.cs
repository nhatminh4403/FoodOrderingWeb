using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FoodOrderingWeb.Models
{
    public class FoodItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int FoodId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc"), 
            StringLength(maximumLength:  100000,MinimumLength =2, ErrorMessage = "Tên sản phẩm phải ít nhất 2 ký tự")]
        public string? FoodName { get; set; }


        [Required(ErrorMessage ="Không được để trống")]
        [Range(1000,1000000000000000000,ErrorMessage ="Giá sản phẩm ít nhất từ 1000 Vnđ trở lên")]
        public double? FoodPrice { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [DisplayName("Mô tả")]
        public string? FoodDescription { get; set; }

        [DisplayName("Ảnh đại diện")]
        public string? MainPictureUrl { get; set; }
        public int? CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        public virtual ICollection<Restaurant>? Restaurants { get; set; }
        public virtual ICollection<PictureLists>? PictureLists { get; set; }
    }
}

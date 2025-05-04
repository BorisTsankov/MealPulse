using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class FoodItem
    {
        [Key]
        public int FoodItemId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Calories { get; set; }

        [Required]
        public decimal Protein { get; set; }

        [Required]
        public decimal Fat { get; set; }

        [Required]
        public decimal Carbohydrates { get; set; }

        [Required]
        public decimal Sugars { get; set; }

        [Required]
        public decimal Fiber { get; set; }

        public decimal? Sodium { get; set; }

        public decimal? Potassium { get; set; }

        public decimal? Iron { get; set; }

        public decimal? Calcium { get; set; }

        [Required]
        [StringLength(50)]
        public string Unit { get; set; } = "g";  // g, ml, piece etc.

        public ICollection<FoodDiaryItem> FoodDiaryItems { get; set; } = new List<FoodDiaryItem>();
    }
}

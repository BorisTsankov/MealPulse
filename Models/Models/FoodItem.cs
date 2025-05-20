using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class FoodItem
    {
        public int FoodItemId { get; set; }

        [Required, StringLength(255)]
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
        public string Unit { get; set; } = "g";

        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Barcode { get; set; }
        public string? Source { get; set; } // e.g., "OpenFoodFacts"
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}

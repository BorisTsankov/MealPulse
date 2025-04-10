using static MealPulse.Common.ValidationConstraints.FoodItem;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace MealPulse.Models.Models
{
    public class FoodItem
    {
        [Key]
        public int FoodItemId { get; set; }

        [Required]
        [StringLength(FoodItemNameMaxLenght)]
        public string FoodItemName { get; set; } = null!;

        [StringLength(BrandMaxLength)]
        public string? Brand { get; set; }

        [Range(DefaultServingQuantityMin, DefaultServingQuantityMax)]
        public int ServingG {  get; set; }

        [Range(CaloriesMin, double.MaxValue)]
        public double CaloriesPer100g { get; set; }

        [Range(CaloriesMin, double.MaxValue)]
        public double CaloriesPer100ml { get; set; }

        [Range(ProteinMin, double.MaxValue)]
        public double Protein_g { get; set; }

        [Range(FatsMin, double.MaxValue)]
        public double Fats_g { get; set; }

        [Range(CarbsMin, double.MaxValue)]
        public double Carbs_g { get; set; }

        public ICollection<FoodDiaryItem> FoodDiaryItems { get; set; } = new List<FoodDiaryItem>();
    }
}

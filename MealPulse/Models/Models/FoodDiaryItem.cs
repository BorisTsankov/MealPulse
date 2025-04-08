using static MealPulse.Common.ValidationConstraints;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace MealPulse.Models.Models
{
    public class FoodDiaryItem
    {
        [Key]
        public int FoodDiaryItemId { get; set; }

        [ForeignKey(nameof(Goal))]
        public int GoalId { get; set; }

        [ForeignKey(nameof(FoodItem))]
        public int FoodId { get; set; }

        [ForeignKey(nameof(MealType))]
        public int MealTypeId { get; set; }

        public DateTime DateTime { get; set; }

        public double Quantity { get; set; }

        public Goal Goal { get; set; } = null!;
        public FoodItem FoodItem { get; set; } = null!;
        public MealType MealType { get; set; } = null!;
    }
}


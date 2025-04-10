using static MealPulse.Common.ValidationConstraints.MealType;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MealPulse.Models.Models
{
    public class MealType
    {
        [Key]
        public int MealTypeId { get; set; }

        [Required]
        [StringLength(MealTypeNameMaxLength)]
        public string MealTypeName { get; set; } = null!;

        public ICollection<FoodDiaryItem> FoodDiaryItems { get; set; } = new List<FoodDiaryItem>();
    }
}

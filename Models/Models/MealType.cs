using System.ComponentModel.DataAnnotations;
using static Common.ValidationConstraints.MealType;

namespace Models.Models
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

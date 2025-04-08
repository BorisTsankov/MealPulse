using static MealPulse.Common.ValidationConstraints.Goal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace MealPulse.Models.Models
{
    public class Goal
    {
        [Key]
        public int GoalId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Range(WeightMin, WeightMax)]
        public double CurrentWeightKg { get; set; }

        [Range(WeightMin, WeightMax)]
        public double TargetWeightKg { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public User User { get; set; } = null!;
        public ICollection<FoodDiaryItem> FoodDiaryItems { get; set; } = new List<FoodDiaryItem>();
    }
}


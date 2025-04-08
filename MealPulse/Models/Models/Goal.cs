using static MealPulse.Common.ValidationConstraints.Goal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace MealPulse.Models.Models
{
    public class Goal
    {
        [Key]
        public int goal_id { get; set; }

        [ForeignKey(nameof(User))]
        public int user_id { get; set; }

        [Range(WeightMin, WeightMax)]
        public double current_weight_kg { get; set; }

        [Range(WeightMin, WeightMax)]
        public double target_weight_kg { get; set; }

        public bool is_active { get; set; }

        public DateTime start_date { get; set; }
        public DateTime? end_date { get; set; }

        public User User { get; set; } = null!;
        public ICollection<FoodDiaryItem> FoodDiaryItems { get; set; } = new List<FoodDiaryItem>();
    }
}


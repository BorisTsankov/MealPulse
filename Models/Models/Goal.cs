using Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.ValidationConstraints.Goal;

namespace Models.Models
{
    public class Goal
    {
        [Key]
        public int goal_id { get; set; }

        [ForeignKey(nameof(User))]
        public int user_id { get; set; }

        [Range(WeightMin, WeightMax)]
        public decimal current_weight_kg { get; set; }

        [Range(WeightMin, WeightMax)]
        public decimal target_weight_kg { get; set; }

        public bool is_active { get; set; }

        public DateTime start_date { get; set; }
        public DateTime? end_date { get; set; }

        public int goal_intensity { get; set; }

        public GoalIntensity IntensityEnum => (GoalIntensity)goal_intensity;

        public User User { get; set; } = null!;
        public ICollection<FoodDiaryItem> FoodDiaryItems { get; set; } = new List<FoodDiaryItem>();
    }
}


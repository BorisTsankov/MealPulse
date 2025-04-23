using MealPulse.Models.Models;

namespace MealPulse.ViewModels
{
    public class UserProfileViewModel
    {
        public User User { get; set; }
        public Goal? Goal { get; set; }
        public string GoalIntensityDisplay { get; set; }
        public int? DailyCalories { get; set; }


        // NEW: Display names
        public string GenderName { get; set; }
        public string ActivityLevelName { get; set; }
        public string MetricName { get; set; }

    }


}

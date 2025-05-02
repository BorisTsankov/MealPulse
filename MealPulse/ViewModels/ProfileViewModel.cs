using Services.Models;

namespace MealPulse.ViewModels
{
    public class UserProfileViewModel
    {
        public UserDto User { get; set; } = null!;
        public GoalDto? Goal { get; set; }

        public string GoalIntensityDisplay { get; set; } = "";
        public int? DailyCalories { get; set; }

        public string GenderName => User.GenderName;
        public string ActivityLevelName => User.ActivityLevelName;
        public string MetricName => User.MetricName;
    }
}

using MealPulse.Models.Models;

namespace MealPulse.ViewModels
{
    public class UserProfileViewModel
    {
        public User User { get; set; }
        public Goal? Goal { get; set; } // Optional: show current goal or null
    }
}

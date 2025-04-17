using MealPulse.Models.Models;

namespace Services.Services.Interfaces
{
    public interface IGoalService
    {
        Goal? GetMostRecentGoalByUserId(int userId);
        bool UpdateWeight(int userId, decimal newWeight);
        Goal? CreateNewGoal(int userId);
    }
}

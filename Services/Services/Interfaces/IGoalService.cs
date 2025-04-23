using MealPulse.Models.Models;

namespace Services.Services.Interfaces
{
    public interface IGoalService
    {
        Goal? GetMostRecentGoalByUserId(int userId);
        bool UpdateWeight(int userId, decimal newWeight);
        bool CreateGoal(Goal newGoal); // Return bool to confirm success
        int? CalculateCalorieGoal(User user, Goal goal);


    }
}

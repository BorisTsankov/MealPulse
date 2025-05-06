using DTOs.DTOs;

namespace Services.Services.Interfaces
{
    public interface IGoalService
    {
        GoalDto? GetMostRecentGoalByUserId(int userId);
        bool UpdateWeight(int userId, decimal newWeight);
        bool CreateGoal(int userId, decimal currentWeight, decimal targetWeight, string intensity);
        int? CalculateCalorieGoal(UserDto user, GoalDto goal);



    }
}

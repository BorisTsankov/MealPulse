using Models.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IGoalRepository
    {
        Goal? GetMostRecentGoalByUserId(int userId);
        bool UpdateWeight(int userId, decimal newWeight);
        bool CreateGoal(Goal goal);

    }
}

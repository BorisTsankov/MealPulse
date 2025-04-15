using DataAccess.Repositories.Interfaces;
using MealPulse.Models.Models;
using Services.Services.Interfaces;

namespace Services.Services
{
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;

        public GoalService(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public Goal? GetMostRecentGoalByUserId(int userId)
        {
            return _goalRepository.GetMostRecentGoalByUserId(userId);
        }

        public bool UpdateWeight(int userId, decimal newWeight)
        {
            return _goalRepository.UpdateWeight(userId, newWeight);
        }
    }
}

using DataAccess.Repositories.Interfaces;
using MealPulse.Data;
using MealPulse.Models.Models;
using Services.Services.Interfaces;
using System.Collections.Generic;

namespace Services.Services
{
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;
        private readonly DbHelper _dbHelper; // Assuming _dbHelper is injected

        public GoalService(IGoalRepository goalRepository, DbHelper dbHelper)
        {
            _goalRepository = goalRepository;
            _dbHelper = dbHelper;
        }

        public Goal? GetMostRecentGoalByUserId(int userId)
        {
            return _goalRepository.GetMostRecentGoalByUserId(userId);
        }

        public bool UpdateWeight(int userId, decimal newWeight)
        {
            return _goalRepository.UpdateWeight(userId, newWeight);
        }

        public bool CreateGoal(Goal newGoal)
        {
            return _goalRepository.CreateGoal(newGoal);
        }
    }
}

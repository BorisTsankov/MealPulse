using Core.Models.Enums;
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
        private readonly DbHelper _dbHelper;
        private readonly IGenderService _genderService;
        private readonly IActivityLevelService _activityLevelService;


        public GoalService(
            IGoalRepository goalRepository,
            DbHelper dbHelper,
            IGenderService genderService,
            IActivityLevelService activityLevelService)
        {
            _goalRepository = goalRepository;
            _dbHelper = dbHelper;
            _genderService = genderService;
            _activityLevelService = activityLevelService;
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

        public int? CalculateCalorieGoal(User user, Goal goal)
        {
            if (user.height_cm == null || goal?.current_weight_kg == null || user.date_of_birth == null)
                return null;

            var gender = _genderService.GetGenderName(user.gender_id);
            var activityLevel = _activityLevelService.GetActivityLevelName(user.activityLevel_id);
            var goalIntensity = (GoalIntensity)goal.goal_intensity;

            int age = DateTime.Today.Year - user.date_of_birth.Value.Year;
            if (user.date_of_birth > DateTime.Today.AddYears(-age)) age--;

            double bmr = (10 * (double)goal.current_weight_kg) +
                         (6.25 * (double)user.height_cm) -
                         (5 * age) +
                         (gender?.Trim().ToLower() == "male" ? 5 : -161);

            double multiplier = activityLevel.ToLower() switch
            {
                "not active" => 1.2,
                "slightly active" => 1.375,
                "average" => 1.55,
                "above average" => 1.725,
                "very active" => 1.9,
                _ => 1.2
            };

            double tdee = bmr * multiplier;

            int adjustment = goalIntensity switch
            {
                GoalIntensity.LoseQuarterKgPerWeek => -250,
                GoalIntensity.LoseHalfKgPerWeek => -500,
                GoalIntensity.LoseOneKgPerWeek => -1000,
                GoalIntensity.GainQuarterKgPerWeek => 250,
                GoalIntensity.GainHalfKgPerWeek => 500,
                GoalIntensity.GainOneKgPerWeek => 1000,
                _ => 0
            };

            return (int)Math.Round(tdee + adjustment);
        }

    }
}

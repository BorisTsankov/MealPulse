using Core.Models.Enums;
using DataAccess.Repositories.Interfaces;
using MealPulse.Data;
using Services.Mappers;
using Services.Models;
using Services.Services.Interfaces;

namespace Services.Services
{
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;
        private readonly IGenderService _genderService;
        private readonly IActivityLevelService _activityLevelService;

        public GoalService(
            IGoalRepository goalRepository,
            IGenderService genderService,
            IActivityLevelService activityLevelService)
        {
            _goalRepository = goalRepository;
            _genderService = genderService;
            _activityLevelService = activityLevelService;
        }

        public GoalDto? GetMostRecentGoalByUserId(int userId)
        {
            var goal = _goalRepository.GetMostRecentGoalByUserId(userId);
            return goal != null ? GoalMapper.ToDto(goal) : null;
        }

        public bool UpdateWeight(int userId, decimal newWeight)
        {
            return _goalRepository.UpdateWeight(userId, newWeight);
        }

        public bool CreateGoal(int userId, decimal currentWeight, decimal targetWeight, string intensity)
        {
            var newGoal = new DataAccess.Models.Goal
            {
                user_id = userId,
                current_weight_kg = currentWeight,
                target_weight_kg = targetWeight,
                start_date = DateTime.Today,
                goal_intensity = (int)Enum.Parse<GoalIntensity>(intensity),
                is_active = true
            };

            return _goalRepository.CreateGoal(newGoal);
        }

        public int? CalculateCalorieGoal(UserDto user, GoalDto goal)
        {
            if (goal.CurrentWeightKg == 0 || user.HeightCm == 0 || user.DateOfBirth == null)
                return null;

            var gender = user.GenderName;
            var activityLevel = user.ActivityLevelName;
            var goalIntensity = (GoalIntensity)goal.GoalIntensity;

            int age = DateTime.Today.Year - user.DateOfBirth.Value.Year;
            if (user.DateOfBirth > DateTime.Today.AddYears(-age)) age--;

            double bmr = (10 * (double)goal.CurrentWeightKg) +
                         (6.25 * (double)user.HeightCm) -
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

using DTOs.DTOs;
using Models.Models;

namespace Services.Mappers
{
    public static class GoalMapper
    {
        public static GoalDto ToDto(Goal goal)
        {
            return new GoalDto
            {
                GoalId = goal.goal_id,
                UserId = goal.user_id,
                CurrentWeightKg = goal.current_weight_kg,
                TargetWeightKg = goal.target_weight_kg,
                IsActive = goal.is_active,
                StartDate = goal.start_date,
                EndDate = goal.end_date,
                GoalIntensity = goal.goal_intensity
            };
        }
    }
}

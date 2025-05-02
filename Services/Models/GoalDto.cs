using System;

namespace Services.Models
{
    public class GoalDto
    {
        public int GoalId { get; set; }
        public int UserId { get; set; }

        public decimal CurrentWeightKg { get; set; }
        public decimal TargetWeightKg { get; set; }

        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int GoalIntensity { get; set; }
        public string GoalIntensityDisplay => ((Core.Models.Enums.GoalIntensity)GoalIntensity).ToString();
    }
}

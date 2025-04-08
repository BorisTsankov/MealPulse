using static MealPulse.Common.ValidationConstraints.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace MealPulse.Models.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Auth0Id { get; set; } = null!;

        [Required]
        [StringLength(FirstnameMaxLength)]
        public string Firstname { get; set; } = null!;

        [Required]
        [StringLength(LastnameMaxLength)]
        public string Lastname { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength)]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [Range(AgeMin, AgeMax)]
        public int Age { get; set; }

        [Required]
        [Range(HeightMin, HeightMax)]
        public double HeightCm { get; set; }

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }

        [ForeignKey(nameof(Gender))]
        public int GenderId { get; set; }

        [ForeignKey(nameof(ActivityLevel))]
        public int ActivityLevelId { get; set; }

        [ForeignKey(nameof(Metric))]
        public int MetricId { get; set; }

        public Role Role { get; set; } = null!;
        public Gender Gender { get; set; } = null!;
        public ActivityLevel ActivityLevel { get; set; } = null!;
        public Metric Metric { get; set; } = null!;

        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
    }
}
    


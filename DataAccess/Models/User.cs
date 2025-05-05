using static DataAccess.Common.ValidationConstraints.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [Required]
        [StringLength(FirstnameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastnameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength)]
        public string email { get; set; } = null!;

        [Required]
        public string password { get; set; } = null!;

        [Required]
        [Range(AgeMin, AgeMax)]
        public DateTime? date_of_birth { get; set; }

        [Required]
        public int gender_id { get; set; }

        [Required]
        [Range(HeightMin, HeightMax)]
        public decimal height_cm { get; set; }

        [Required]
        public int activityLevel_id { get; set; }

        [Required]
        public int metric_id { get; set; }

        [ForeignKey(nameof(Role))]
        public int role_id { get; set; }

        public Role Role { get; set; } = null!;
        public Gender Gender { get; set; } = null!;
        public ActivityLevel ActivityLevel { get; set; } = null!;
        public Metric Metric { get; set; } = null!;

        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
    }
    //public class LoginViewModel
    //{
    //    [Required]
    //    [EmailAddress]
    //    public string Email { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }
    //}
}

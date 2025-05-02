using static MealPulse.Common.ValidationConstraints.ActivityLevel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class ActivityLevel
    {
        [Key]
        public int ActivityLevelId { get; set; }

        [Required]
        [StringLength(ActivityLevelNameMaxLength)]
        public string ActivityLevelName { get; set; } = null!;

        public ICollection<User> Users { get; set; } = new List<User>();  
    }
}

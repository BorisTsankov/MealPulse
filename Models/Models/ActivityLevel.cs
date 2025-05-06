using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class ActivityLevel
    {
        [Key]
        public int ActivityLevelId { get; set; }

        [Required]
        [StringLength(Common.ValidationConstraints.ActivityLevel.ActivityLevelNameMaxLength)]
        public string ActivityLevelName { get; set; } = null!;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

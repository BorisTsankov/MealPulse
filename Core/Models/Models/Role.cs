using static MealPulse.Common.ValidationConstraints.Role;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MealPulse.Models.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(RoleNameMaxLength)]
        public string RoleName { get; set; } = null!;

        public ICollection<User> User { get; set; } = new List<User>();
    }
}

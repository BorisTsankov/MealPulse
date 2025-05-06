using System.ComponentModel.DataAnnotations;
using static Common.ValidationConstraints.Role;

namespace Models.Models
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

using System.ComponentModel.DataAnnotations;
using static Common.ValidationConstraints.Gender;

namespace Models.Models
{
    public class Gender
    {
        [Key]
        public int GenderId { get; set; }

        [Required]
        [StringLength(GenderNameMaxLength)]
        public string GenderName { get; set; } = null!;

        public ICollection<User> Users { get; set; } = new List<User>();

    }
}

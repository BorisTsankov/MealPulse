using static MealPulse.Common.ValidationConstraints.Gender;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace MealPulse.Models.Models
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

using static MealPulse.Common.ValidationConstraints.Metric;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace MealPulse.Models.Models
{
    public class Metric
    {
        [Key]
        public int MetricId { get; set; }

        [Required]
        [StringLength(MetricNameMaxLength)]
        public string MetricName { get; set; } = null!;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

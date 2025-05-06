using System.ComponentModel.DataAnnotations;
using static Common.ValidationConstraints.Metric;

namespace Models.Models
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

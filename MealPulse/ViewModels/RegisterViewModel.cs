using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Height is required.")]
        [Range(120, 300, ErrorMessage = "Height must be between 120 and 300 cm.")]
        public decimal? HeightCm { get; set; }

        [Required(ErrorMessage = "Weight is required.")]
        [Range(40, 300, ErrorMessage = "Weight must be between 40 and 300 kg.")]
        public decimal? WeightKg { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public int? GenderId { get; set; }

        [Required(ErrorMessage = "Activity level is required.")]
        public int? ActivityLevelId { get; set; }

        [Required(ErrorMessage = "Measurement system is required.")]
        public int? MetricId { get; set; }
    }
}

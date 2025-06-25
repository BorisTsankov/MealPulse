using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Height is required.")]
        [Range(120, 300, ErrorMessage = "Height must be between 120 and 300 cm.")]
        [Display(Name = "Height (cm)")]
        public decimal? HeightCm { get; set; }

        [Required(ErrorMessage = "Weight is required.")]
        [Range(40, 300, ErrorMessage = "Weight must be between 40 and 300 kg.")]
        [Display(Name = "Weight (kg)")]
        public decimal? WeightKg { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [Display(Name = "Gender")]
        public int? GenderId { get; set; }

        [Required(ErrorMessage = "Activity level is required.")]
        [Display(Name = "Activity Level")]
        public int? ActivityLevelId { get; set; }

        [Required(ErrorMessage = "Measurement system is required.")]
        [Display(Name = "Preferred Unit System")]
        public int? MetricId { get; set; }
    }
}

namespace DTOs.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public decimal HeightCm { get; set; }

        public string GenderName { get; set; } = "Unknown";
        public string ActivityLevelName { get; set; } = "Unknown";
        public string MetricName { get; set; } = "Unknown";

    }
}

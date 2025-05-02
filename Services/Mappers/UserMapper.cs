using DataAccess.Models;
using Services.Models;

namespace Services.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            return new UserDto
            {
                UserId = user.user_id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.email,
                DateOfBirth = user.date_of_birth,
                HeightCm = user.height_cm,
                GenderName = user.Gender?.GenderName ?? "Unknown",
                ActivityLevelName = user.ActivityLevel?.ActivityLevelName ?? "Unknown",
                MetricName = user.Metric?.MetricName ?? "Unknown"
            };
        }
    }
}

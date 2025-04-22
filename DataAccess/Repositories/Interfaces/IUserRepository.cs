using MealPulse.Models.Models;

namespace MealPulse.Data.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserById(int userId);
        void UpdateActivityLevel(int userId, int activityLevelId);
        bool UpdateHeight(int userId, decimal newHeight);
    }
}

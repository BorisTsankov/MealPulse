using MealPulse.Models.Models;

namespace MealPulse.Services.Interfaces
{
    public interface IUserService
    {

        User? GetUserById(int userId);
        bool UpdateHeight(int userId, decimal newHeight);
        void UpdateActivityLevel(int userId, int activityLevelId);

    }
}

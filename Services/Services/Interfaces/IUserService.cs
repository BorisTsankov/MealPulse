using DataAccess.Models;
using Services.Models;

namespace MealPulse.Services.Interfaces
{
    public interface IUserService
    {

        UserDto? GetUserById(int userId);
        bool UpdateHeight(int userId, decimal newHeight);
        void UpdateActivityLevel(int userId, int activityLevelId);

    }
}

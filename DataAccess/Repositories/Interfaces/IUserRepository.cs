using MealPulse.Models.Models;

namespace MealPulse.Data.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserById(int userId);
        bool UpdateHeight(int userId, decimal newHeight);
    }
}

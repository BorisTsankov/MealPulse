using MealPulse.Models.Models;
using MealPulse.Services.Interfaces;
using MealPulse.Data.Interfaces;

namespace MealPulse.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? GetUserById(int userId)
        {
            return _userRepository.GetUserById(userId);
        }

        public bool UpdateHeight(int userId, decimal newHeight)
        {
            return _userRepository.UpdateHeight(userId, newHeight);
        }

        
    }
}

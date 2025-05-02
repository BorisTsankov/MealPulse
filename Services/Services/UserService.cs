using MealPulse.Services.Interfaces;
using MealPulse.Data.Interfaces;
using DataAccess.Models;
using Services.Mappers;
using Services.Models;

namespace MealPulse.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDto? GetUserById(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            return user != null ? UserMapper.ToDto(user) : null;
        }

        public bool UpdateHeight(int userId, decimal newHeight)
        {
            return _userRepository.UpdateHeight(userId, newHeight);
        }
        public void UpdateActivityLevel(int userId, int activityLevelId)
        {
            _userRepository.UpdateActivityLevel(userId, activityLevelId);
        }



    }
}

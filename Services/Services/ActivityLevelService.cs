using DataAccess.Repositories.Interfaces;
using Models.Models;
using Services.Services.Interfaces;

namespace Services.Services
{
    public class ActivityLevelService : IActivityLevelService
    {
        private readonly IActivityLevelRepository _activityLevelRepository;

        public ActivityLevelService(IActivityLevelRepository activityLevelRepository)
        {
            _activityLevelRepository = activityLevelRepository;
        }

        public List<ActivityLevel> GetAll()
        {
            return _activityLevelRepository.GetAll();
        }

        public string GetActivityLevelName(int id)
        {
            return _activityLevelRepository.GetActivityLevelNameById(id);
        }
    }
}
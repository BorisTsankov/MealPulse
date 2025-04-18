using DataAccess.Repositories.Interfaces;
using MealPulse.Models.Models;
using Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
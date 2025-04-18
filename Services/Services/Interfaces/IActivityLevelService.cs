using MealPulse.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Interfaces
{
    public interface IActivityLevelService
    {
        List<ActivityLevel> GetAll();
        string GetActivityLevelName(int id);
    }
}

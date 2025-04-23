using MealPulse.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IGoalRepository
    {
        Goal? GetMostRecentGoalByUserId(int userId);
        bool UpdateWeight(int userId, decimal newWeight);
        bool CreateGoal(Goal goal);

    }
}

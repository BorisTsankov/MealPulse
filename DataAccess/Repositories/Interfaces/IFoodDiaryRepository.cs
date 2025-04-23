using MealPulse.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IFoodDiaryRepository
    {
        List<FoodDiaryItem> GetItemsByGoalId(int goalId);
        bool Add(FoodDiaryItem item);
        List<FoodDiaryItem> GetItemsByGoalIdAndDate(int goalId, DateTime date);

    }
}

using MealPulse.Models.Models;
using System.Collections.Generic;

namespace Services.Services.Interfaces
{
    public interface IFoodDiaryService
    {
        List<FoodDiaryItem> GetItemsForGoal(int goalId);
        bool AddFoodDiaryItem(FoodDiaryItem item);

    }
}

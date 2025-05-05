using DataAccess.Models;
using Services.Models;
using System.Collections.Generic;

namespace Services.Services.Interfaces
{
    public interface IFoodDiaryService
    {
        List<FoodDiaryItem> GetItemsForGoal(int goalId);
        bool AddFoodDiaryItem(FoodDiaryItemDto dto);
        List<FoodDiaryItem> GetItemsForGoalAndDate(int goalId, DateTime date);
        bool DeleteFoodDiaryItem(int foodDiaryItemId);

    }
}

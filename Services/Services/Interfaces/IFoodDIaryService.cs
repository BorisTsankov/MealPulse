using DTOs.DTOs;
using Models.Models;


namespace Services.Services.Interfaces
{
    public interface IFoodDiaryService
    {
        List<FoodDiaryItem> GetItemsForGoal(int goalId);
        bool AddFoodDiaryItem(FoodDiaryItemDto dto);
        List<FoodDiaryItem> GetItemsForGoalAndDate(int goalId, DateTime date);
        bool DeleteFoodDiaryItem(int foodDiaryItemId);
        //List<FoodDiaryItem> GetItemsByUserAndDate(int userId, DateTime date);

    }
}

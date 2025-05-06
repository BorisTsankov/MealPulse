using Models.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IFoodDiaryRepository
    {
        List<FoodDiaryItem> GetItemsByGoalId(int goalId);
        bool Add(FoodDiaryItem item);
        List<FoodDiaryItem> GetItemsByGoalIdAndDate(int goalId, DateTime date);
        bool Delete(int foodDiaryItemId);
        List<FoodDiaryItem> GetItemsByUserIdAndDate(int userId, DateTime date);

    }
}

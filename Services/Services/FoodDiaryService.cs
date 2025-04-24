using DataAccess.Repositories.Interfaces;
using MealPulse.Models.Models;
using Services.Services.Interfaces;
using System.Collections.Generic;

namespace Services.Services
{
    public class FoodDiaryService : IFoodDiaryService
    {
        private readonly IFoodDiaryRepository _foodDiaryRepository;

        public FoodDiaryService(IFoodDiaryRepository foodDiaryRepository)
        {
            _foodDiaryRepository = foodDiaryRepository;
        }

        public List<FoodDiaryItem> GetItemsForGoal(int goalId)
        {
            return _foodDiaryRepository.GetItemsByGoalId(goalId);
        }

        public bool AddFoodDiaryItem(FoodDiaryItem item)
        {
            return _foodDiaryRepository.Add(item);
        }

        public List<FoodDiaryItem> GetItemsForGoalAndDate(int goalId, DateTime date)
        {
            return _foodDiaryRepository.GetItemsByGoalIdAndDate(goalId, date);
        }

        public bool DeleteFoodDiaryItem(int foodDiaryItemId)
        {
            return _foodDiaryRepository.Delete(foodDiaryItemId);
        }
    }
}

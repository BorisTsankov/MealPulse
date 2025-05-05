using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Services.Mappers;
using Services.Models;
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

        public bool AddFoodDiaryItem(FoodDiaryItemDto dto)
        {
            var entity = FoodDiaryItemMapper.ToEntity(dto);
            return _foodDiaryRepository.Add(entity);
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

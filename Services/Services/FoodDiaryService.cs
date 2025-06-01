using DataAccess.Repositories.Interfaces;
using DTOs.DTOs;
using Models.Models;
using Services.Mappers;
using Services.Services.Interfaces;

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

        //public List<FoodDiaryItem> GetItemsByUserAndDate(int userId, DateTime date)
        //{
        //    return _foodDiaryRepository.GetItemsByUserIdAndDate(userId, date);
        //}

    }
}

using DTOs.DTOs;
using Models.Models;

namespace Services.Mappers
{
    public static class FoodDiaryItemMapper
    {
        public static FoodDiaryItemDto ToDto(FoodDiaryItem item)
        {
            return new FoodDiaryItemDto
            {
                FoodDiaryItemId = item.FoodDiaryItemId,
                GoalId = item.GoalId,
                FoodId = item.FoodId,
                MealTypeId = item.MealTypeId,
                DateTime = item.DateTime,
                Quantity = item.Quantity,
                FoodName = item.FoodItem?.Name,
                MealTypeName = item.MealType?.MealTypeName
            };
        }

        public static FoodDiaryItem ToEntity(FoodDiaryItemDto dto)
        {
            return new FoodDiaryItem
            {
                FoodDiaryItemId = dto.FoodDiaryItemId,
                GoalId = dto.GoalId,
                FoodId = dto.FoodId,
                MealTypeId = dto.MealTypeId,
                DateTime = dto.DateTime,
                Quantity = dto.Quantity
            };
        }
    }
}
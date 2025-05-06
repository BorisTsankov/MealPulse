using DTOs.DTOs;
using Models.Models;

namespace Services.Mappers
{
    public static class FoodItemMapper
    {
        public static FoodItemDto ToDto(FoodItem item)
        {
            return new FoodItemDto
            {
                FoodItemId = item.FoodItemId,
                Name = item.Name,
                Unit = item.Unit,
                Calories = item.Calories,
                Protein = item.Protein,
                Fat = item.Fat,
                Carbohydrates = item.Carbohydrates,
                Sugars = item.Sugars,
                Fiber = item.Fiber,
                Sodium = item.Sodium ?? 0,
                Potassium = item.Potassium ?? 0,
                Iron = item.Iron ?? 0,
                Calcium = item.Calcium ?? 0
            };
        }
    }
}
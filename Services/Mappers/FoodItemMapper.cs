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
                Sodium = item.Sodium,   
                Potassium = item.Potassium,
                Iron = item.Iron,
                Calcium = item.Calcium,
                Description = item.Description,  
                Brand = item.Brand,
                Barcode = item.Barcode,
                Source = item.Source,
                ImageUrl = item.ImageUrl,
                CreatedAt = item.CreatedAt
            };
        }
        public static FoodItem ToEntity(FoodItemDto dto)
        {
            return new FoodItem
            {
                FoodItemId = dto.FoodItemId ?? 0,
                Name = dto.Name,
                Unit = dto.Unit,
                Calories = dto.Calories,
                Protein = dto.Protein,
                Fat = dto.Fat,
                Carbohydrates = dto.Carbohydrates,
                Sugars = dto.Sugars,
                Fiber = dto.Fiber,
                Sodium = dto.Sodium,
                Potassium = dto.Potassium,
                Iron = dto.Iron,
                Calcium = dto.Calcium,
                CreatedAt = DateTime.UtcNow,
                Description = dto.Description,
                Brand = dto.Brand,
                Barcode = dto.Barcode,
                Source = dto.Source,
                ImageUrl = dto.ImageUrl,
            };
        }

    }
}
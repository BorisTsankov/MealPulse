using DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class FoodLoggingHelper
    {
        public static FoodDiaryItemDto ConvertToDiaryItem(ChatFoodLog log, int goalId)
        {
            return new FoodDiaryItemDto
            {
                GoalId = goalId,
                MealTypeId = GetMealTypeId(log.mealType),
                DateTime = DateTime.UtcNow,
                Quantity = log.quantity,
                FoodName = log.foodName,
                FoodItem = new FoodItemDto
                {
                    Name = log.foodName,
                    Unit = log.unit,
                    Calories = log.calories,
                    Protein = log.protein,
                    Fat = log.fat,
                    Carbohydrates = log.carbohydrates,
                    Sugars = log.sugars,
                    Fiber = log.fiber,
                    Sodium = log.sodium,
                    Potassium = log.potassium,
                    Iron = log.iron,
                    Calcium = log.calcium
                }
            };
        }

        public static int GetMealTypeId(string mealType)
        {
            if (string.IsNullOrWhiteSpace(mealType))
                throw new ArgumentException("Meal type is required.");

            return mealType.ToLower() switch
            {
                "breakfast" => 1,
                "lunch" => 2,
                "dinner" => 3,
                "snack" => 4,
                _ => throw new ArgumentException($"Invalid meal type: {mealType}")
            };
        }

    }
}

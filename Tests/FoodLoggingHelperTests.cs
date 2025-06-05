using DTOs.DTOs;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Helpers
{
    public class FoodLoggingHelperTests
    {
        [Fact]
        public void GetMealTypeId_ShouldReturnCorrectId_ForKnownTypes()
        {
            Assert.Equal(1, FoodLoggingHelper.GetMealTypeId("breakfast"));
            Assert.Equal(2, FoodLoggingHelper.GetMealTypeId("lunch"));
            Assert.Equal(3, FoodLoggingHelper.GetMealTypeId("dinner"));
            Assert.Equal(4, FoodLoggingHelper.GetMealTypeId("snack"));
        }

        [Fact]
        public void GetMealTypeId_ShouldBeCaseInsensitive()
        {
            Assert.Equal(1, FoodLoggingHelper.GetMealTypeId("BREAKFAST"));
            Assert.Equal(2, FoodLoggingHelper.GetMealTypeId("Lunch"));
        }

        [Fact]
        public void GetMealTypeId_ShouldDefaultToSnack_ForUnknownTypes()
        {
            Assert.Equal(4, FoodLoggingHelper.GetMealTypeId("random"));
            Assert.Equal(4, FoodLoggingHelper.GetMealTypeId(""));
        }

        [Fact]
        public void ConvertToDiaryItem_ShouldMapAllPropertiesCorrectly()
        {
            // Arrange
            var input = new ChatFoodLog
            {
                foodName = "banana",
                quantity = 150,
                unit = "g",
                mealType = "lunch",
                calories = 89,
                protein = 1.1m,
                fat = 0.3m,
                carbohydrates = 22.8m,
                sugars = 12.2m,
                fiber = 2.6m,
                sodium = 1,
                potassium = 358,
                iron = 0.3m,
                calcium = 5
            };

            int goalId = 42;

            // Act
            var result = FoodLoggingHelper.ConvertToDiaryItem(input, goalId);

            // Assert
            Assert.Equal(goalId, result.GoalId);
            Assert.Equal(2, result.MealTypeId); // lunch → 2
            Assert.Equal(input.foodName, result.FoodName);
            Assert.Equal(input.quantity, result.Quantity);
            Assert.NotNull(result.FoodItem);

            Assert.Equal("banana", result.FoodItem.Name);
            Assert.Equal("g", result.FoodItem.Unit);
            Assert.Equal(89, result.FoodItem.Calories);
            Assert.Equal(1.1m, result.FoodItem.Protein);
            Assert.Equal(0.3m, result.FoodItem.Fat);
            Assert.Equal(22.8m, result.FoodItem.Carbohydrates);
            Assert.Equal(12.2m, result.FoodItem.Sugars);
            Assert.Equal(2.6m, result.FoodItem.Fiber);
            Assert.Equal(1, result.FoodItem.Sodium);
            Assert.Equal(358, result.FoodItem.Potassium);
            Assert.Equal(0.3m, result.FoodItem.Iron);
            Assert.Equal(5, result.FoodItem.Calcium);

            Assert.True(result.DateTime <= DateTime.UtcNow);
        }

        [Fact]
        public void ConvertToDiaryItem_ShouldHandleUnknownMealType()
        {
            var input = new ChatFoodLog
            {
                foodName = "apple",
                quantity = 100,
                unit = "g",
                mealType = "dessert",
                calories = 52,
                protein = 0.3m,
                fat = 0.2m,
                carbohydrates = 14m,
                sugars = 10m,
                fiber = 2.4m,
                sodium = 1,
                potassium = 107,
                iron = 0.1m,
                calcium = 6
            };

            var result = FoodLoggingHelper.ConvertToDiaryItem(input, 1);

            // Default to snack (4)
            Assert.Equal(4, result.MealTypeId);
        }
    }
}
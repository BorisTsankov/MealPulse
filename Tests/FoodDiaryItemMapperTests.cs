using DTOs.DTOs;
using Models.Models;
using Services.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Mappers
{
    public class FoodDiaryItemMapperTests
    {
        [Fact]
        public void ToEntity_ShouldMapAllSimpleFields()
        {
            // Arrange
            var dto = new FoodDiaryItemDto
            {
                FoodDiaryItemId = 1,
                GoalId = 2,
                FoodId = 3,
                MealTypeId = 4,
                DateTime = new DateTime(2024, 1, 1),
                Quantity = 150
            };

            // Act
            var result = FoodDiaryItemMapper.ToEntity(dto);

            // Assert
            Assert.Equal(1, result.FoodDiaryItemId);
            Assert.Equal(2, result.GoalId);
            Assert.Equal(3, result.FoodId);
            Assert.Equal(4, result.MealTypeId);
            Assert.Equal(new DateTime(2024, 1, 1), result.DateTime);
            Assert.Equal(150, result.Quantity);
        }

        [Fact]
        public void ToDto_ShouldMapAllFields_WhenNestedObjectsExist()
        {
            // Arrange
            var item = new FoodDiaryItem
            {
                FoodDiaryItemId = 1,
                GoalId = 2,
                FoodId = 3,
                MealTypeId = 4,
                DateTime = new DateTime(2024, 5, 5),
                Quantity = 200,
                FoodItem = new FoodItem
                {
                    FoodItemId = 10,
                    Name = "Egg",
                    Unit = "g",
                    Calories = 155,
                    Protein = 13,
                    Fat = 11,
                    Carbohydrates = 1.1m
                },
                MealType = new MealType
                {
                    MealTypeId = 4,
                    MealTypeName = "Snack"
                }
            };

            // Act
            var dto = FoodDiaryItemMapper.ToDto(item);

            // Assert
            Assert.Equal(item.FoodDiaryItemId, dto.FoodDiaryItemId);
            Assert.Equal(item.GoalId, dto.GoalId);
            Assert.Equal(item.FoodId, dto.FoodId);
            Assert.Equal(item.MealTypeId, dto.MealTypeId);
            Assert.Equal(item.DateTime, dto.DateTime);
            Assert.Equal(item.Quantity, dto.Quantity);

            Assert.Equal("Egg", dto.FoodName);
            Assert.Equal("Snack", dto.MealTypeName);
            Assert.NotNull(dto.FoodItem);
            Assert.Equal(155, dto.FoodItem.Calories);
        }

        [Fact]
        public void ToDto_ShouldHandleNullNestedObjects()
        {
            // Arrange
            var item = new FoodDiaryItem
            {
                FoodDiaryItemId = 1,
                GoalId = 2,
                FoodId = 3,
                MealTypeId = 4,
                DateTime = DateTime.Now,
                Quantity = 100,
                FoodItem = null,
                MealType = null
            };

            // Act
            var dto = FoodDiaryItemMapper.ToDto(item);

            // Assert
            Assert.Null(dto.FoodItem);
            Assert.Null(dto.FoodName);
            Assert.Null(dto.MealTypeName);
        }
    }
}


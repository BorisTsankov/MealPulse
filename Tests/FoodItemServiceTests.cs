using DataAccess.Repositories.Interfaces;
using DTOs.DTOs;
using Models.Models;
using Moq;
using Services.Services;

namespace Tests
{
    public class FoodItemServiceTests
    {
        private readonly Mock<IFoodItemRepository> _mockRepo;
        private readonly FoodItemService _service;

        public FoodItemServiceTests()
        {
            _mockRepo = new Mock<IFoodItemRepository>();
            _service = new FoodItemService(_mockRepo.Object);
        }

        [Fact]
        public void GetAll_ReturnsListOfFoodItemDtos()
        {
            // Arrange
            var foodItems = new List<FoodItem>
            {
                new FoodItem { FoodItemId = 1, Name = "Apple", Calories = 52 },
                new FoodItem { FoodItemId = 2, Name = "Banana", Calories = 89 }
            };

            _mockRepo.Setup(r => r.GetAll()).Returns(foodItems);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.IsType<FoodItemDto>(item));
            Assert.Equal("Apple", result[0].Name);
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetById_ExistingId_ReturnsDto()
        {
            // Arrange
            var foodItem = new FoodItem { FoodItemId = 1, Name = "Yogurt", Calories = 59 };
            _mockRepo.Setup(r => r.GetById(1)).Returns(foodItem);

            // Act
            var result = _service.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FoodItemDto>(result);
            Assert.Equal("Yogurt", result!.Name);
            _mockRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public void GetById_NonExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((FoodItem?)null);

            // Act
            var result = _service.GetById(99);

            // Assert
            Assert.Null(result);
            _mockRepo.Verify(r => r.GetById(99), Times.Once);
        }

        [Fact]
        public void SearchByName_ReturnsMatchingDtos()
        {
            // Arrange
            var foodItems = new List<FoodItem>
            {
                new FoodItem { FoodItemId = 1, Name = "Chicken Breast", Calories = 165 },
                new FoodItem { FoodItemId = 2, Name = "Grilled Chicken", Calories = 170 }
            };

            _mockRepo.Setup(r => r.SearchByName("chicken")).Returns(foodItems);

            // Act
            var result = _service.SearchByName("chicken");

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.Contains("chicken", item.Name.ToLower()));
            _mockRepo.Verify(r => r.SearchByName("chicken"), Times.Once);
        }
    }
}
using DataAccess.Repositories.Interfaces;
using Models.Models;
using Moq;
using Services.Services;

namespace Tests
{
    public class MealTypeServiceTests
    {
        private readonly Mock<IMealTypeRepository> _mockRepo;
        private readonly MealTypeService _service;

        public MealTypeServiceTests()
        {
            _mockRepo = new Mock<IMealTypeRepository>();
            _service = new MealTypeService(_mockRepo.Object);
        }

        [Fact]
        public void GetAll_ReturnsAllMealTypes()
        {
            // Arrange
            var mealTypes = new List<MealType>
            {
                new MealType { MealTypeId = 1, MealTypeName = "Breakfast" },
                new MealType { MealTypeId = 2, MealTypeName = "Lunch" },
                new MealType { MealTypeId = 3, MealTypeName = "Dinner" },
                new MealType { MealTypeId = 4, MealTypeName = "Snack" }
            };

            _mockRepo.Setup(r => r.GetAll()).Returns(mealTypes);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Contains(result, m => m.MealTypeName == "Lunch");
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }
    }
}
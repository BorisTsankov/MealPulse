using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Moq;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
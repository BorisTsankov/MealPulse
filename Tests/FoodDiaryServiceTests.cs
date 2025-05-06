using DataAccess.Repositories.Interfaces;
using Models.Models;
using Moq;
using Services.Services;

namespace Tests
{
    public class FoodDiaryServiceTests
    {
        private readonly Mock<IFoodDiaryRepository> _mockRepo;
        private readonly FoodDiaryService _service;

        public FoodDiaryServiceTests()
        {
            _mockRepo = new Mock<IFoodDiaryRepository>();
            _service = new FoodDiaryService(_mockRepo.Object);
        }

        [Fact]
        public void GetItemsForGoal_ReturnsCorrectItems()
        {
            // Arrange
            int goalId = 1;
            var items = new List<FoodDiaryItem>
            {
                new FoodDiaryItem { FoodDiaryItemId = 1, GoalId = goalId, FoodId = 1 },
                new FoodDiaryItem { FoodDiaryItemId = 2, GoalId = goalId, FoodId = 2 }
            };

            _mockRepo.Setup(r => r.GetItemsByGoalId(goalId)).Returns(items);

            // Act
            var result = _service.GetItemsForGoal(goalId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.Equal(goalId, item.GoalId));
            _mockRepo.Verify(r => r.GetItemsByGoalId(goalId), Times.Once);
        }

        //[Fact]
        //public void AddFoodDiaryItem_ValidItem_ReturnsTrue()
        //{
        //    // Arrange
        //    var item = new FoodDiaryItem { FoodDiaryItemId = 1, GoalId = 1, FoodId = 1 };
        //    _mockRepo.Setup(r => r.Add(item)).Returns(true);

        //    // Act
        //    var result = _service.AddFoodDiaryItem(item);

        //    // Assert
        //    Assert.True(result);
        //    _mockRepo.Verify(r => r.Add(item), Times.Once);
        //}

        [Fact]
        public void GetItemsForGoalAndDate_ReturnsCorrectItems()
        {
            // Arrange
            int goalId = 1;
            DateTime date = new DateTime(2025, 5, 1);
            var items = new List<FoodDiaryItem>
            {
                new FoodDiaryItem { FoodId = 1, GoalId = goalId, DateTime = date },
                new FoodDiaryItem { FoodId = 2, GoalId = goalId, DateTime = date }
            };

            _mockRepo.Setup(r => r.GetItemsByGoalIdAndDate(goalId, date)).Returns(items);

            // Act
            var result = _service.GetItemsForGoalAndDate(goalId, date);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, item =>
            {
                Assert.Equal(goalId, item.GoalId);
                Assert.Equal(date, item.DateTime);
            });
            _mockRepo.Verify(r => r.GetItemsByGoalIdAndDate(goalId, date), Times.Once);
        }

        [Fact]
        public void DeleteFoodDiaryItem_ExistingId_ReturnsTrue()
        {
            // Arrange
            int id = 42;
            _mockRepo.Setup(r => r.Delete(id)).Returns(true);

            // Act
            var result = _service.DeleteFoodDiaryItem(id);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.Delete(id), Times.Once);
        }
    }
}
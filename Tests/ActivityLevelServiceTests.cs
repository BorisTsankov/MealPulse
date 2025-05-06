using DataAccess.Repositories.Interfaces;
using Models.Models;
using Moq;
using Services.Services;

namespace Tests
{
    public class ActivityLevelServiceTests
    {
        private readonly Mock<IActivityLevelRepository> _mockRepo;
        private readonly ActivityLevelService _service;

        public ActivityLevelServiceTests()
        {
            _mockRepo = new Mock<IActivityLevelRepository>();
            _service = new ActivityLevelService(_mockRepo.Object);
        }

        [Fact]
        public void GetAll_ReturnsListOfActivityLevels()
        {
            // Arrange
            var levels = new List<ActivityLevel>
            {
                new ActivityLevel { ActivityLevelId = 1, ActivityLevelName = "Low" },
                new ActivityLevel { ActivityLevelId = 2, ActivityLevelName = "High" }
            };

            _mockRepo.Setup(r => r.GetAll()).Returns(levels);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, l => l.ActivityLevelName == "Low");
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetActivityLevelName_ValidId_ReturnsCorrectName()
        {
            // Arrange
            int id = 1;
            string expectedName = "Moderate";

            _mockRepo.Setup(r => r.GetActivityLevelNameById(id)).Returns(expectedName);

            // Act
            var result = _service.GetActivityLevelName(id);

            // Assert
            Assert.Equal(expectedName, result);
            _mockRepo.Verify(r => r.GetActivityLevelNameById(id), Times.Once);
        }
    }
}
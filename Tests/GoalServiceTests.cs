using Common;
using DataAccess.Repositories.Interfaces;
using DTOs.DTOs;
using Models.Models;
using Moq;
using Services.Services;
using Services.Services.Interfaces;

namespace Tests
{
    public class GoalServiceTests
    {
        private readonly Mock<IGoalRepository> _goalRepoMock = new();
        private readonly Mock<IGenderService> _genderServiceMock = new();
        private readonly Mock<IActivityLevelService> _activityServiceMock = new();
        private readonly GoalService _service;

        public GoalServiceTests()
        {
            _service = new GoalService(_goalRepoMock.Object, _genderServiceMock.Object, _activityServiceMock.Object);

        }

        [Fact]
        public void GetMostRecentGoalByUserId_ReturnsDto()
        {
            _goalRepoMock.Setup(r => r.GetMostRecentGoalByUserId(1)).Returns(new Goal { goal_id = 123, user_id = 1 });

            var result = _service.GetMostRecentGoalByUserId(1);

            Assert.NotNull(result);
            Assert.Equal(123, result.GoalId);
        }

        [Fact]
        public void UpdateWeight_CallsRepository()
        {
            _goalRepoMock.Setup(r => r.UpdateWeight(1, 75)).Returns(true);

            var result = _service.UpdateWeight(1, 75);

            Assert.True(result);
        }

        [Fact]
        public void CreateGoal_ReturnsTrue_WhenValid()
        {
            _goalRepoMock.Setup(r => r.CreateGoal(It.IsAny<Goal>())).Returns(true);

            var result = _service.CreateGoal(1, 80, 70, "LoseHalfKgPerWeek");

            Assert.True(result);
        }

        [Fact]
        public void CalculateCalorieGoal_ReturnsNull_WhenMissingValues()
        {
            var user = new UserDto { HeightCm = 0, DateOfBirth = null };
            var goal = new GoalDto { CurrentWeightKg = 0 };

            var result = _service.CalculateCalorieGoal(user, goal);

            Assert.Null(result);
        }

        [Fact]
        public void CalculateCalorieGoal_ReturnsCalories()
        {
            var user = new UserDto
            {
                HeightCm = 180,
                DateOfBirth = new DateTime(2000, 1, 1),
                GenderName = "male",
                ActivityLevelName = "average"
            };

            var goal = new GoalDto
            {
                CurrentWeightKg = 75,
                GoalIntensity = (int)GoalIntensity.LoseHalfKgPerWeek
            };

            var result = _service.CalculateCalorieGoal(user, goal);

            Assert.NotNull(result);
            Assert.InRange(result.Value, 1000, 5000);
        }
    }
}
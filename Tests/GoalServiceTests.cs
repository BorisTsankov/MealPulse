using Common;
using DataAccess.Repositories.Interfaces;
using DTOs.DTOs;
using Moq;
using Services.Services.Interfaces;
using Services.Services;
using Models.Models;

public class GoalServiceTests
{
    private readonly Mock<IGoalRepository> _goalRepoMock = new();
    private readonly Mock<IGenderService> _genderServiceMock = new();
    private readonly Mock<IActivityLevelService> _activityLevelServiceMock = new();
    private readonly GoalService _goalService;

    public GoalServiceTests()
    {
        _goalService = new GoalService(_goalRepoMock.Object, _genderServiceMock.Object, _activityLevelServiceMock.Object);
    }

    [Fact]
    public void GetMostRecentGoalByUserId_ValidUserId_ReturnsDto()
    {
        var goal = new Goal { goal_id = 1, user_id = 1, current_weight_kg = 70, target_weight_kg = 65 };
        _goalRepoMock.Setup(r => r.GetMostRecentGoalByUserId(1)).Returns(goal);

        var result = _goalService.GetMostRecentGoalByUserId(1);

        Assert.NotNull(result);
        Assert.Equal(70, result!.CurrentWeightKg);
    }

    [Fact]
    public void GetMostRecentGoalByUserId_NoGoal_ReturnsNull()
    {
        _goalRepoMock.Setup(r => r.GetMostRecentGoalByUserId(2)).Returns((Goal?)null);

        var result = _goalService.GetMostRecentGoalByUserId(2);

        Assert.Null(result);
    }

    [Fact]
    public void UpdateWeight_ValidInput_ReturnsTrue()
    {
        _goalRepoMock.Setup(r => r.UpdateWeight(1, 75)).Returns(true);

        var result = _goalService.UpdateWeight(1, 75);

        Assert.True(result);
    }

    [Fact]
    public void UpdateWeight_UpdateFails_ReturnsFalse()
    {
        _goalRepoMock.Setup(r => r.UpdateWeight(1, 75)).Returns(false);

        var result = _goalService.UpdateWeight(1, 75);

        Assert.False(result);
    }

    [Fact]
    public void CreateGoal_ValidInput_ReturnsTrue()
    {
        _goalRepoMock.Setup(r => r.CreateGoal(It.IsAny<Goal>())).Returns(true);

        var result = _goalService.CreateGoal(1, 70, 60, "LoseHalfKgPerWeek");

        Assert.True(result);
    }

    [Fact]
    public void CreateGoal_InvalidEnum_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => _goalService.CreateGoal(1, 70, 60, "INVALID_ENUM"));
    }

    [Fact]
    public void CalculateCalorieGoal_ValidInput_ReturnsCorrectCalories()
    {
        var user = new UserDto
        {
            HeightCm = 180,
            GenderName = "Male",
            ActivityLevelName = "Average",
            DateOfBirth = new DateTime(2000, 1, 1)
        };

        var goal = new GoalDto
        {
            CurrentWeightKg = 70,
            GoalIntensity = (int)GoalIntensity.LoseHalfKgPerWeek
        };

        var calories = _goalService.CalculateCalorieGoal(user, goal);

        Assert.NotNull(calories);
        Assert.InRange(calories!.Value, 1000, 4000); // Reasonable calorie range
    }

    [Theory]
    [InlineData(0, 180)] // Weight = 0
    [InlineData(70, 0)]  // Height = 0
    public void CalculateCalorieGoal_ZeroInputs_ReturnsNull(decimal weight, int height)
    {
        var user = new UserDto
        {
            HeightCm = height,
            GenderName = "Male",
            ActivityLevelName = "Average",
            DateOfBirth = new DateTime(2000, 1, 1)
        };

        var goal = new GoalDto
        {
            CurrentWeightKg = weight,
            GoalIntensity = (int)GoalIntensity.Maintain
        };

        var result = _goalService.CalculateCalorieGoal(user, goal);

        Assert.Null(result);
    }

    [Fact]
    public void CalculateCalorieGoal_NullDateOfBirth_ReturnsNull()
    {
        var user = new UserDto
        {
            HeightCm = 170,
            GenderName = "Female",
            ActivityLevelName = "Average",
            DateOfBirth = null
        };

        var goal = new GoalDto
        {
            CurrentWeightKg = 60,
            GoalIntensity = (int)GoalIntensity.LoseHalfKgPerWeek
        };

        var result = _goalService.CalculateCalorieGoal(user, goal);

        Assert.Null(result);
    }

    [Fact]
    public void CalculateCalorieGoal_UnknownActivityLevel_UsesDefaultMultiplier()
    {
        var user = new UserDto
        {
            HeightCm = 170,
            GenderName = "Female",
            ActivityLevelName = "unicorn mode",
            DateOfBirth = new DateTime(1995, 1, 1)
        };

        var goal = new GoalDto
        {
            CurrentWeightKg = 60,
            GoalIntensity = (int)GoalIntensity.Maintain
        };

        var result = _goalService.CalculateCalorieGoal(user, goal);

        Assert.NotNull(result);
    }
}

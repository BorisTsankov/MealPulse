using DataAccess.Repositories.Interfaces;
using DTOs.DTOs;
using Models.Models;
using Moq;
using Services.Services;

public class FoodDiaryServiceTests
{
    private readonly Mock<IFoodDiaryRepository> _repoMock = new();
    private readonly FoodDiaryService _service;

    public FoodDiaryServiceTests()
    {
        _service = new FoodDiaryService(_repoMock.Object);
    }

    // -------------------------
    // GetItemsForGoal
    // -------------------------

    [Fact]
    public void GetItemsForGoal_ItemsExist_ReturnsList()
    {
        var goalId = 1;
        var items = new List<FoodDiaryItem> { new() { GoalId = goalId, FoodId = 1 } };
        _repoMock.Setup(r => r.GetItemsByGoalId(goalId)).Returns(items);

        var result = _service.GetItemsForGoal(goalId);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(goalId, result[0].GoalId);
    }

    [Fact]
    public void GetItemsForGoal_NoItems_ReturnsEmptyList()
    {
        _repoMock.Setup(r => r.GetItemsByGoalId(1)).Returns(new List<FoodDiaryItem>());

        var result = _service.GetItemsForGoal(1);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    // -------------------------
    // AddFoodDiaryItem
    // -------------------------

    [Fact]
    public void AddFoodDiaryItem_ValidDto_ReturnsTrue()
    {
        var dto = new FoodDiaryItemDto
        {
            GoalId = 1,
            FoodId = 2,
            Quantity = 100,
            MealTypeId = 3,
            DateTime = DateTime.Today
        };

        _repoMock.Setup(r => r.Add(It.IsAny<FoodDiaryItem>())).Returns(true);

        var result = _service.AddFoodDiaryItem(dto);

        Assert.True(result);
    }

    [Fact]
    public void AddFoodDiaryItem_InsertFails_ReturnsFalse()
    {
        var dto = new FoodDiaryItemDto
        {
            GoalId = 1,
            FoodId = 2,
            Quantity = 100,
            MealTypeId = 3,
            DateTime = DateTime.Today
        };

        _repoMock.Setup(r => r.Add(It.IsAny<FoodDiaryItem>())).Returns(false);

        var result = _service.AddFoodDiaryItem(dto);

        Assert.False(result);
    }

    // -------------------------
    // GetItemsForGoalAndDate
    // -------------------------

    [Fact]
    public void GetItemsForGoalAndDate_ValidInputs_ReturnsItems()
    {
        var date = DateTime.Today;
        var goalId = 1;
        var items = new List<FoodDiaryItem> { new() { GoalId = goalId, DateTime = date } };
        _repoMock.Setup(r => r.GetItemsByGoalIdAndDate(goalId, date)).Returns(items);

        var result = _service.GetItemsForGoalAndDate(goalId, date);

        Assert.Single(result);
        Assert.Equal(goalId, result[0].GoalId);
    }

    [Fact]
    public void GetItemsForGoalAndDate_NoResults_ReturnsEmpty()
    {
        var date = DateTime.Today;
        _repoMock.Setup(r => r.GetItemsByGoalIdAndDate(5, date)).Returns(new List<FoodDiaryItem>());

        var result = _service.GetItemsForGoalAndDate(5, date);

        Assert.Empty(result);
    }

    // -------------------------
    // DeleteFoodDiaryItem
    // -------------------------

    [Fact]
    public void DeleteFoodDiaryItem_ValidId_ReturnsTrue()
    {
        _repoMock.Setup(r => r.Delete(10)).Returns(true);

        var result = _service.DeleteFoodDiaryItem(10);

        Assert.True(result);
    }

    [Fact]
    public void DeleteFoodDiaryItem_Failure_ReturnsFalse()
    {
        _repoMock.Setup(r => r.Delete(10)).Returns(false);

        var result = _service.DeleteFoodDiaryItem(10);

        Assert.False(result);
    }


}

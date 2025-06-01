using DataAccess.Repositories.Interfaces;
using Models.Models;
using Moq;
using Services.Services;

public class MealTypeServiceTests
{
    private readonly Mock<IMealTypeRepository> _repoMock;
    private readonly MealTypeService _service;

    public MealTypeServiceTests()
    {
        _repoMock = new Mock<IMealTypeRepository>();
        _service = new MealTypeService(_repoMock.Object);
    }

    [Fact]
    public void GetAll_ReturnsListOfMealTypes()
    {
        // Arrange
        var mealTypes = new List<MealType>
        {
            new MealType { MealTypeId = 1, MealTypeName = "Breakfast" },
            new MealType { MealTypeId = 2, MealTypeName = "Lunch" }
        };
        _repoMock.Setup(r => r.GetAll()).Returns(mealTypes);

        // Act
        var result = _service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, m => m.MealTypeName == "Breakfast");
        Assert.Contains(result, m => m.MealTypeName == "Lunch");
    }

    [Fact]
    public void GetAll_EmptyList_ReturnsEmptyList()
    {
        // Arrange
        _repoMock.Setup(r => r.GetAll()).Returns(new List<MealType>());

        // Act
        var result = _service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAll_RepositoryReturnsNull_Throws()
    {
        // Arrange
        _repoMock.Setup(r => r.GetAll()).Returns((List<MealType>)null!);

        // Act & Assert
        Assert.Throws<System.NullReferenceException>(() => _service.GetAll());
    }

    [Fact]
    public void GetAll_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        _repoMock.Setup(r => r.GetAll()).Throws(new System.Exception("DB error"));

        // Act & Assert
        var ex = Assert.Throws<System.Exception>(() => _service.GetAll());
        Assert.Equal("DB error", ex.Message);
    }
}

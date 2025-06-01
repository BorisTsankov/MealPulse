using DataAccess.Repositories.Interfaces;
using Models.Models;
using Moq;
using Services.Services;

public class ActivityLevelServiceTests
{
    private readonly Mock<IActivityLevelRepository> _repoMock;
    private readonly ActivityLevelService _service;

    public ActivityLevelServiceTests()
    {
        _repoMock = new Mock<IActivityLevelRepository>();
        _service = new ActivityLevelService(_repoMock.Object);
    }

    // ---------------------------
    // GetAll Tests
    // ---------------------------

    [Fact]
    public void GetAll_ReturnsListOfActivityLevels()
    {
        var levels = new List<ActivityLevel>
        {
            new() { ActivityLevelId = 1, ActivityLevelName = "Not Active" },
            new() { ActivityLevelId = 2, ActivityLevelName = "Slightly Active" }
        };

        _repoMock.Setup(r => r.GetAll()).Returns(levels);

        var result = _service.GetAll();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, l => l.ActivityLevelName == "Not Active");
    }

    [Fact]
    public void GetAll_EmptyList_ReturnsEmpty()
    {
        _repoMock.Setup(r => r.GetAll()).Returns(new List<ActivityLevel>());

        var result = _service.GetAll();

        Assert.Empty(result);
    }

    [Fact]
    public void GetAll_RepositoryReturnsNull_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetAll()).Returns((List<ActivityLevel>?)null);

        var result = _service.GetAll();

        Assert.Null(result);
    }


    // ---------------------------
    // GetActivityLevelName Tests
    // ---------------------------

    [Fact]
    public void GetActivityLevelName_ValidId_ReturnsName()
    {
        _repoMock.Setup(r => r.GetActivityLevelNameById(1)).Returns("Average");

        var result = _service.GetActivityLevelName(1);

        Assert.Equal("Average", result);
    }

    [Fact]
    public void GetActivityLevelName_InvalidId_ReturnsUnknown()
    {
        _repoMock.Setup(r => r.GetActivityLevelNameById(999)).Returns("Unknown");

        var result = _service.GetActivityLevelName(999);

        Assert.Equal("Unknown", result);
    }

    [Fact]
    public void GetActivityLevelName_RepositoryReturnsNull_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetActivityLevelNameById(It.IsAny<int>())).Returns((string?)null);

        var result = _service.GetActivityLevelName(123);

        Assert.Null(result);
    }
}

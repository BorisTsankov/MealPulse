using DataAccess.Repositories.Interfaces;
using Moq;
using Services.Services;
using Models.Models;

public class GenderServiceTests
{
    private readonly Mock<IGenderRepository> _genderRepoMock;
    private readonly GenderService _genderService;

    public GenderServiceTests()
    {
        _genderRepoMock = new Mock<IGenderRepository>();
        _genderService = new GenderService(_genderRepoMock.Object);
    }

    // ---------------------
    // GetAll Tests
    // ---------------------

    [Fact]
    public void GetAll_ReturnsListOfGenders()
    {
        // Arrange
        var genders = new List<Gender>
        {
            new Gender { GenderId = 1, GenderName = "Male" },
            new Gender { GenderId = 2, GenderName = "Female" }
        };
        _genderRepoMock.Setup(repo => repo.GetAll()).Returns(genders);

        // Act
        var result = _genderService.GetAll();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, g => g.GenderName == "Male");
        Assert.Contains(result, g => g.GenderName == "Female");
    }

    [Fact]
    public void GetAll_EmptyList_ReturnsEmptyList()
    {
        _genderRepoMock.Setup(repo => repo.GetAll()).Returns(new List<Gender>());

        var result = _genderService.GetAll();

        Assert.Empty(result);
    }

    [Fact]
    public void GetAll_RepositoryReturnsNull_ThrowsException()
    {
        _genderRepoMock.Setup(repo => repo.GetAll()).Returns((List<Gender>)null!);

        Assert.Throws<System.NullReferenceException>(() => _genderService.GetAll());
    }

    [Fact]
    public void GetAll_RepositoryThrows_PropagatesException()
    {
        _genderRepoMock.Setup(repo => repo.GetAll()).Throws(new System.Exception("DB error"));

        var ex = Assert.Throws<System.Exception>(() => _genderService.GetAll());
        Assert.Equal("DB error", ex.Message);
    }

    // ---------------------
    // GetGenderName Tests
    // ---------------------

    [Fact]
    public void GetGenderName_ValidId_ReturnsCorrectName()
    {
        _genderRepoMock.Setup(repo => repo.GetGenderNameById(1)).Returns("Non-Binary");

        var result = _genderService.GetGenderName(1);

        Assert.Equal("Non-Binary", result);
    }

    [Fact]
    public void GetGenderName_InvalidId_ReturnsNull()
    {
        _genderRepoMock.Setup(repo => repo.GetGenderNameById(999)).Returns((string?)null);

        var result = _genderService.GetGenderName(999);

        Assert.Null(result);
    }

    [Fact]
    public void GetGenderName_RepositoryThrows_PropagatesException()
    {
        _genderRepoMock.Setup(repo => repo.GetGenderNameById(It.IsAny<int>())).Throws(new System.Exception("lookup failed"));

        var ex = Assert.Throws<System.Exception>(() => _genderService.GetGenderName(42));
        Assert.Equal("lookup failed", ex.Message);
    }
}

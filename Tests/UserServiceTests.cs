using DataAccess.Repositories.Interfaces;
using Models.Models;
using Moq;
using Services.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public void GetUserById_UserExists_ReturnsUserDto()
    {
        // Arrange
        var user = new User { user_id = 1, FirstName = "Alice", LastName = "Smith" };
        _userRepositoryMock.Setup(repo => repo.GetUserById(1)).Returns(user);

        // Act
        var result = _userService.GetUserById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Alice", result!.FirstName);
        Assert.Equal("Smith", result.LastName);
    }

    [Fact]
    public void GetUserById_UserDoesNotExist_ReturnsNull()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.GetUserById(2)).Returns((User?)null);

        // Act
        var result = _userService.GetUserById(2);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void UpdateHeight_ValidUserIdAndHeight_ReturnsTrue()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.UpdateHeight(1, 180)).Returns(true);

        // Act
        var result = _userService.UpdateHeight(1, 180);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void UpdateHeight_UpdateFails_ReturnsFalse()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.UpdateHeight(1, 180)).Returns(false);

        // Act
        var result = _userService.UpdateHeight(1, 180);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void UpdateActivityLevel_ValidInput_CallsRepository()
    {
        // Arrange
        var userId = 1;
        var activityLevelId = 2;

        // Act
        _userService.UpdateActivityLevel(userId, activityLevelId);

        // Assert
        _userRepositoryMock.Verify(repo => repo.UpdateActivityLevel(userId, activityLevelId), Times.Once);
    }

    [Fact]
    public void UpdateActivityLevel_CalledWithCorrectParams()
    {
        // Arrange
        var userId = 99;
        var levelId = 4;

        // Act
        _userService.UpdateActivityLevel(userId, levelId);

        // Assert
        _userRepositoryMock.Verify(repo => repo.UpdateActivityLevel(
            It.Is<int>(id => id == 99),
            It.Is<int>(lvl => lvl == 4)
        ), Times.Once);
    }


    // GetUserById edge cases

    [Fact]
    public void GetUserById_NegativeId_ReturnsNull()
    {
        _userRepositoryMock.Setup(r => r.GetUserById(-1)).Returns((User?)null);

        var result = _userService.GetUserById(-1);

        Assert.Null(result);
    }

    [Fact]
    public void GetUserById_IntMaxValue_ReturnsNull()
    {
        _userRepositoryMock.Setup(r => r.GetUserById(int.MaxValue)).Returns((User?)null);

        var result = _userService.GetUserById(int.MaxValue);

        Assert.Null(result);
    }

    [Fact]
    public void GetUserById_RepositoryThrows_ExceptionPropagates()
    {
        _userRepositoryMock.Setup(r => r.GetUserById(1)).Throws(new Exception("DB Failure"));

        Assert.Throws<Exception>(() => _userService.GetUserById(1));
    }

    [Fact]
    public void GetUserById_UserWithNullFields_ReturnsDtoWithDefaults()
    {
        var user = new User { user_id = 1, FirstName = null!, LastName = null! };
        _userRepositoryMock.Setup(r => r.GetUserById(1)).Returns(user);

        var result = _userService.GetUserById(1);

        Assert.NotNull(result);
        Assert.Null(result!.FirstName);
        Assert.Null(result.LastName);
    }

    // UpdateHeight edge cases

    [Fact]
    public void UpdateHeight_ZeroHeight_ReturnsTrueOrFalse()
    {
        _userRepositoryMock.Setup(r => r.UpdateHeight(1, 0)).Returns(true);

        var result = _userService.UpdateHeight(1, 0);

        Assert.True(result);
    }

    [Fact]
    public void UpdateHeight_NegativeHeight_ReturnsFalse()
    {
        _userRepositoryMock.Setup(r => r.UpdateHeight(1, -180)).Returns(false);

        var result = _userService.UpdateHeight(1, -180);

        Assert.False(result);
    }

    [Fact]
    public void UpdateHeight_MaxDecimalHeight_ReturnsTrue()
    {
        decimal maxHeight = decimal.MaxValue;
        _userRepositoryMock.Setup(r => r.UpdateHeight(1, maxHeight)).Returns(true);

        var result = _userService.UpdateHeight(1, maxHeight);

        Assert.True(result);
    }

    [Fact]
    public void UpdateHeight_RepositoryThrows_ExceptionPropagates()
    {
        _userRepositoryMock.Setup(r => r.UpdateHeight(1, 180)).Throws(new Exception("DB Update Failed"));

        Assert.Throws<Exception>(() => _userService.UpdateHeight(1, 180));
    }

    // UpdateActivityLevel edge cases

    [Fact]
    public void UpdateActivityLevel_ZeroLevelId_CallsRepository()
    {
        _userService.UpdateActivityLevel(1, 0);

        _userRepositoryMock.Verify(r => r.UpdateActivityLevel(1, 0), Times.Once);
    }

    [Fact]
    public void UpdateActivityLevel_NegativeLevelId_CallsRepository()
    {
        _userService.UpdateActivityLevel(1, -5);

        _userRepositoryMock.Verify(r => r.UpdateActivityLevel(1, -5), Times.Once);
    }

    [Fact]
    public void UpdateActivityLevel_IntMaxValueLevelId_CallsRepository()
    {
        _userService.UpdateActivityLevel(1, int.MaxValue);

        _userRepositoryMock.Verify(r => r.UpdateActivityLevel(1, int.MaxValue), Times.Once);
    }

    [Fact]
    public void UpdateActivityLevel_RepositoryThrows_ExceptionPropagates()
    {
        _userRepositoryMock
            .Setup(r => r.UpdateActivityLevel(1, 2))
            .Throws(new Exception("Foreign key constraint"));

        Assert.Throws<Exception>(() => _userService.UpdateActivityLevel(1, 2));
    }
}



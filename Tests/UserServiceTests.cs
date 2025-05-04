using DataAccess.Models;
using MealPulse.Data.Interfaces;
using MealPulse.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
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
        public void GetUserById_ShouldReturnDto_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User { user_id = userId, FirstName = "Test", LastName = "User", height_cm = 180 };
            _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns(user);

            // Act
            var result = _userService.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal("Test", result.FirstName);
        }

        [Fact]
        public void GetUserById_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            var userId = 1;
            _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns((User)null!);

            // Act
            var result = _userService.GetUserById(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateHeight_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            var userId = 1;
            var newHeight = 190m;
            _userRepositoryMock.Setup(r => r.UpdateHeight(userId, newHeight)).Returns(true);

            // Act
            var result = _userService.UpdateHeight(userId, newHeight);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateActivityLevel_ShouldCallRepository()
        {
            // Arrange
            var userId = 1;
            var activityLevelId = 3;

            // Act
            _userService.UpdateActivityLevel(userId, activityLevelId);

            // Assert
            _userRepositoryMock.Verify(r => r.UpdateActivityLevel(userId, activityLevelId), Times.Once);
        }

        [Fact]
        public void GetUserById_ShouldReturnNull_WhenUserIdIsZero()
        {
            // Arrange
            int userId = 0;
            _userRepositoryMock.Setup(r => r.GetUserById(userId)).Returns((User?)null);

            // Act
            var result = _userService.GetUserById(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            int nonExistentUserId = 99999;
            _userRepositoryMock.Setup(r => r.GetUserById(nonExistentUserId)).Returns((User?)null);

            // Act
            var result = _userService.GetUserById(nonExistentUserId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateHeight_ShouldReturnFalse_WhenUserIdIsInvalid()
        {
            // Arrange
            int userId = -1;
            decimal newHeight = 175;
            _userRepositoryMock.Setup(r => r.UpdateHeight(userId, newHeight)).Returns(false);

            // Act
            var result = _userService.UpdateHeight(userId, newHeight);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UpdateHeight_ShouldHandleExtremeHeightValues()
        {
            // Arrange
            int userId = 1;
            decimal extremelyTall = 300;
            _userRepositoryMock.Setup(r => r.UpdateHeight(userId, extremelyTall)).Returns(true);

            // Act
            var result = _userService.UpdateHeight(userId, extremelyTall);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateActivityLevel_ShouldNotThrow_WhenActivityLevelIdIsNegative()
        {
            // Arrange
            int userId = 1;
            int invalidActivityLevelId = -5;

            _userRepositoryMock.Setup(r => r.UpdateActivityLevel(userId, invalidActivityLevelId));

            // Act + Assert
            var ex = Record.Exception(() => _userService.UpdateActivityLevel(userId, invalidActivityLevelId));
            Assert.Null(ex);
        }
    }

}
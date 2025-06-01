using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Services.Services.Interfaces;
using Services.Services;
using System.Data;
using System.Security.Claims;
using Models.Models;
using Common;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _authRepoMock = new();
    private readonly Mock<IHttpContextAccessor> _httpContextMock = new();
    private readonly Mock<IGoalRepository> _goalRepoMock = new();
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService(
            _authRepoMock.Object,
            _httpContextMock.Object,
            _goalRepoMock.Object,
            _emailServiceMock.Object
        );
    }

    [Fact]
    public void UserExists_ExistingUser_ReturnsTrue()
    {
        _authRepoMock.Setup(r => r.UserExists("test@example.com")).Returns(true);
        Assert.True(_authService.UserExists("test@example.com"));
    }

    [Fact]
    public void HashPassword_ReturnsHashedString()
    {
        var hashed = _authService.HashPassword("MyPassword123");
        Assert.False(string.IsNullOrWhiteSpace(hashed));
        Assert.NotEqual("MyPassword123", hashed);
    }

    [Fact]
    public void RegisterUser_SuccessfullyRegisters_CreatesInitialGoal()
    {
        var parameters = new Dictionary<string, object>
        {
            ["@weight_kg"] = 75m
        };

        _authRepoMock.Setup(r => r.RegisterUser(parameters)).Returns(1);
        _goalRepoMock.Setup(r => r.CreateGoal(It.IsAny<Goal>())).Returns(true);

        int result = _authService.RegisterUser(parameters);

        Assert.Equal(1, result);
        _goalRepoMock.Verify(r => r.CreateGoal(It.Is<Goal>(g =>
            g.user_id == 1 &&
            g.current_weight_kg == 75 &&
            g.target_weight_kg == 75 &&
            g.goal_intensity == (int)GoalIntensity.Maintain)), Times.Once);
    }

    [Fact]
    public void RegisterUser_FailsToRegister_DoesNotCreateGoal()
    {
        var parameters = new Dictionary<string, object> { ["@weight_kg"] = 75m };
        _authRepoMock.Setup(r => r.RegisterUser(parameters)).Returns(0);

        int result = _authService.RegisterUser(parameters);

        Assert.Equal(0, result);
        _goalRepoMock.Verify(r => r.CreateGoal(It.IsAny<Goal>()), Times.Never);
    }

    [Fact]
    public void AuthenticateUser_ValidCredentials_CallsAuthRepositoryWithHashedPassword()
    {
        string email = "user@example.com";
        string password = "Secure123";

        _authRepoMock.Setup(r => r.AuthenticateUser(email, It.IsAny<string>()))
                     .Returns(new DataTable());

        var result = _authService.AuthenticateUser(email, password);

        Assert.IsType<DataTable>(result);
        _authRepoMock.Verify(r => r.AuthenticateUser(email, It.Is<string>(s => s != password)), Times.Once);
    }

    [Fact]
    public void GetSelectListData_ReturnsListOfSelectListItems()
    {
        var dt = new DataTable();
        dt.Columns.Add("id");
        dt.Columns.Add("label");
        var row = dt.NewRow();
        row["id"] = "1";
        row["label"] = "Test";
        dt.Rows.Add(row);

        _authRepoMock.Setup(r => r.GetSelectListData("table", "id", "label")).Returns(dt);

        var result = _authService.GetSelectListData("table", "id", "label");

        Assert.Single(result);
        Assert.Equal("1", result[0].Value);
        Assert.Equal("Test", result[0].Text);
    }

    [Fact]
    public void GetCurrentUserId_UserAuthenticated_ReturnsUserId()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "42")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = user };
        _httpContextMock.Setup(h => h.HttpContext).Returns(httpContext);

        string userId = _authService.GetCurrentUserId();

        Assert.Equal("42", userId);
    }

    [Fact]
    public void GetCurrentUserId_NoUser_ReturnsEmpty()
    {
        _httpContextMock.Setup(h => h.HttpContext).Returns((HttpContext?)null);

        string result = _authService.GetCurrentUserId();

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ConfirmUserEmail_ValidToken_CallsRepo()
    {
        _authRepoMock.Setup(r => r.ConfirmEmail("abc123")).Returns(true);

        var result = _authService.ConfirmUserEmail("abc123");

        Assert.True(result);
        _authRepoMock.Verify(r => r.ConfirmEmail("abc123"), Times.Once);
    }

    [Fact]
    public void SendResetEmail_TokenGenerated_EmailSent()
    {
        var email = "reset@example.com";

        _authRepoMock.Setup(r => r.SetPasswordResetToken(
            email,
            It.IsAny<string>(),
            It.IsAny<DateTime>())).Returns(true);

        var mockContext = new DefaultHttpContext();
        mockContext.Request.Scheme = "https";
        mockContext.Request.Host = new HostString("localhost");

        _httpContextMock.Setup(h => h.HttpContext).Returns(mockContext);



        _httpContextMock.Setup(h => h.HttpContext).Returns(mockContext);

        var result = _authService.SendResetEmail(email);

        Assert.True(result);
        _emailServiceMock.Verify(e => e.SendPasswordResetEmail(email, It.Is<string>(link => link.Contains("/Auth/ResetPassword"))), Times.Once);
    }

    [Fact]
    public void SendResetEmail_TokenSetupFails_ReturnsFalse()
    {
        _authRepoMock.Setup(r => r.SetPasswordResetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                     .Returns(false);

        var result = _authService.SendResetEmail("fail@example.com");

        Assert.False(result);
        _emailServiceMock.Verify(e => e.SendPasswordResetEmail(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void ResetPassword_ValidToken_ReturnsTrue()
    {
        _authRepoMock.Setup(r => r.ResetPassword("token123", It.IsAny<string>())).Returns(true);

        var result = _authService.ResetPassword("token123", "NewPass!123");

        Assert.True(result);
    }

    [Fact]
    public void ResetPassword_Failure_ReturnsFalse()
    {
        _authRepoMock.Setup(r => r.ResetPassword("badtoken", It.IsAny<string>())).Returns(false);

        var result = _authService.ResetPassword("badtoken", "NewPass!123");

        Assert.False(result);
    }
}

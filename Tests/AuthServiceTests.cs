using Xunit;
using Moq;
using MealPulse.Services;
using MealPulse.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _authRepoMock;
    private readonly Mock<IHttpContextAccessor> _httpContextMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authRepoMock = new Mock<IAuthRepository>();
        _httpContextMock = new Mock<IHttpContextAccessor>();
        _authService = new AuthService(_authRepoMock.Object, _httpContextMock.Object);
    }

    [Fact]
    public void UserExists_ShouldReturnTrue_WhenUserExists()
    {
        _authRepoMock.Setup(r => r.UserExists("test@example.com")).Returns(true);
        var result = _authService.UserExists("test@example.com");
        Assert.True(result);
    }

    [Fact]
    public void HashPassword_ShouldReturnValidHash()
    {
        var hash = _authService.HashPassword("mypassword");
        Assert.False(string.IsNullOrEmpty(hash));
    }

    [Fact]
    public void RegisterUser_ShouldCallRepositoryAndReturnId()
    {
        var fakeParams = new Dictionary<string, object> { { "Email", "test@example.com" } };
        _authRepoMock.Setup(r => r.RegisterUser(fakeParams)).Returns(42);

        var result = _authService.RegisterUser(fakeParams);

        Assert.Equal(42, result);
    }

    [Fact]
    public void AuthenticateUser_ShouldCallRepositoryWithHashedPassword()
    {
        var email = "test@example.com";
        var password = "secret";
        var expectedTable = new DataTable();
        _authRepoMock.Setup(r => r.AuthenticateUser(email, It.IsAny<string>())).Returns(expectedTable);

        var result = _authService.AuthenticateUser(email, password);

        Assert.Same(expectedTable, result);
    }

    [Fact]
    public void GetSelectListData_ShouldMapDataTableToSelectListItems()
    {
        var table = new DataTable();
        table.Columns.Add("Id");
        table.Columns.Add("Name");
        table.Rows.Add("1", "Option A");

        _authRepoMock.Setup(r => r.GetSelectListData("Users", "Id", "Name")).Returns(table);

        var list = _authService.GetSelectListData("Users", "Id", "Name");

        Assert.Single(list);
        Assert.Equal("1", list[0].Value);
        Assert.Equal("Option A", list[0].Text);
    }

    [Fact]
    public void GetCurrentUserId_ShouldReturnEmpty_WhenHttpContextIsNull()
    {
        _httpContextMock.Setup(h => h.HttpContext).Returns((HttpContext)null);

        var result = _authService.GetCurrentUserId();

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void AuthenticateUser_ShouldHashEmptyPassword_AndReturnFromRepository()
    {
        var email = "test@example.com";
        var password = "";
        var expectedTable = new DataTable();
        _authRepoMock.Setup(r => r.AuthenticateUser(email, It.IsAny<string>()))
                     .Returns(expectedTable);

        var result = _authService.AuthenticateUser(email, password);

        Assert.Same(expectedTable, result);
    }

    [Fact]
    public void GetSelectListData_ShouldReturnEmptyList_WhenNoRows()
    {
        var emptyTable = new DataTable();
        emptyTable.Columns.Add("Id");
        emptyTable.Columns.Add("Name");

        _authRepoMock.Setup(r => r.GetSelectListData("AnyTable", "Id", "Name"))
                     .Returns(emptyTable);

        var result = _authService.GetSelectListData("AnyTable", "Id", "Name");

        Assert.Empty(result);
    }

    [Fact]
    public void GetCurrentUserId_ShouldReturnUserId_WhenUserIsAuthenticated()
    {
        var userId = "123";
        var claims = new List<System.Security.Claims.Claim>
    {
        new(System.Security.Claims.ClaimTypes.NameIdentifier, userId)
    };

        var identity = new System.Security.Claims.ClaimsIdentity(claims, "TestAuthType");
        var principal = new System.Security.Claims.ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = principal };
        _httpContextMock.Setup(h => h.HttpContext).Returns(httpContext);

        var result = _authService.GetCurrentUserId();

        Assert.Equal("123", result);
    }

}

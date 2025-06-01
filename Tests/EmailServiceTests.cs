using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Services.Services;
using System;

public class EmailServiceTests
{
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly Mock<EmailService> _emailServiceMock;

    public EmailServiceTests()
    {
        _emailServiceMock = new Mock<EmailService>(_configMock.Object) { CallBase = true };
    }

    [Fact]
    public void SendConfirmationEmail_CallsSendEmail_WithExpectedContent()
    {
        string to = "user@example.com";
        string link = "https://mealpulse.com/confirm?token=123";

        _emailServiceMock.Setup(s => s.SendEmail(
            to,
            "Confirm Your MealPulse Account",
            It.Is<string>(body => body.Contains(link) && body.Contains("Confirm Email"))
        )).Verifiable();

        _emailServiceMock.Object.SendConfirmationEmail(to, link);

        _emailServiceMock.Verify();
    }

    [Fact]
    public void SendGoalReminderEmail_CallsSendEmail_WithCalories()
    {
        string to = "user@example.com";
        string name = "Alex";
        int remainingCalories = 420;

        _emailServiceMock.Setup(s => s.SendEmail(
            to,
            "MealPulse Daily Reminder",
            It.Is<string>(body => body.Contains("420") && body.Contains("Hi Alex"))
        )).Verifiable();

        _emailServiceMock.Object.SendGoalReminderEmail(to, name, remainingCalories);

        _emailServiceMock.Verify();
    }

    [Fact]
    public void SendPasswordResetEmail_CallsSendEmail_ContainsLink()
    {
        string to = "user@example.com";
        string link = "https://mealpulse.com/reset?token=abc";

        // Create dummy logo
        string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Clean_logo.png");
        Directory.CreateDirectory(Path.GetDirectoryName(logoPath)!);
        File.WriteAllBytes(logoPath, new byte[] { 0x89, 0x50, 0x4E }); // fake PNG header

        _emailServiceMock.Setup(s => s.SendEmail(
            to,
            "Reset Your MealPulse Password",
            It.Is<string>(body => body.Contains(link) && body.Contains("Reset Password"))
        )).Verifiable();

        _emailServiceMock.Object.SendPasswordResetEmail(to, link);

        _emailServiceMock.Verify();
    }

    [Fact]
    public void GetEmailFooter_ContainsCurrentYear()
    {
        var service = new EmailService(_configMock.Object);
        var footer = service.GetType()
                            .GetMethod("GetEmailFooter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                            .Invoke(service, null) as string;

        Assert.Contains(DateTime.Now.Year.ToString(), footer);
    }

    [Fact]
    public void GetEmailHeader_ReturnsValidImageTag()
    {
        // Create dummy image
        string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Clean_logo.png");
        Directory.CreateDirectory(Path.GetDirectoryName(imgPath)!);
        File.WriteAllBytes(imgPath, new byte[] { 0x89, 0x50, 0x4E });

        var service = new EmailService(_configMock.Object);
        var header = service.GetType()
                            .GetMethod("GetEmailHeader", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                            .Invoke(service, null) as string;

        Assert.Contains("data:image/png;base64", header);
        Assert.Contains("MealPulse Logo", header);
    }
}

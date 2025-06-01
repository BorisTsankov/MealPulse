using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Services.Services;
using Services.Services.Interfaces;
using RestSharp;
using System.Net;
using System.Threading.Tasks;

public class AiServiceTests
{
    [Fact]
    public async Task AskAsync_WithEmptyPrompt_ReturnsErrorMessage()
    {
        var config = new Mock<IConfiguration>();
        var client = new Mock<IAiHttpClient>();

        var service = new AiService(config.Object, client.Object);

        var result = await service.AskAsync("");

        Assert.Equal("Please ask a non-empty question.", result);
    }

    [Fact]
    public async Task AskAsync_ResponseSuccess_ParsesContentCorrectly()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["OpenAI:ApiKey"]).Returns("fake-key");

        var response = new RestResponse
        {
            StatusCode = HttpStatusCode.OK,
            Content = @"{""choices"":[{""message"":{""content"":""Hello, I'm MealPulseBot!""}}]}",
            ResponseStatus = ResponseStatus.Completed,
            IsSuccessStatusCode = true
        };


        var client = new Mock<IAiHttpClient>();
        client.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>()))
              .ReturnsAsync(response);

        var service = new AiService(config.Object, client.Object);

        var result = await service.AskAsync("Who are you?");

        Assert.Equal("Hello, I'm MealPulseBot!", result);

    }

    [Fact]
    public async Task AskAsync_ApiFails_ReturnsFailureMessage()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["OpenAI:ApiKey"]).Returns("fake-key");

        var response = new RestResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            ErrorMessage = "Invalid request",
            ResponseStatus = ResponseStatus.Error,
            IsSuccessStatusCode = false
        };


        var client = new Mock<IAiHttpClient>();
        client.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>()))
              .ReturnsAsync(response);

        var service = new AiService(config.Object, client.Object);

        var result = await service.AskAsync("anything");

        Assert.Contains("API request failed", result);
        Assert.Contains("Invalid request", result);
    }

    [Fact]
    public async Task AskAsync_MalformedJson_ReturnsErrorMessage()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["OpenAI:ApiKey"]).Returns("fake-key");

        var response = new RestResponse
        {
            StatusCode = HttpStatusCode.OK,
            Content = "not-json",
            ResponseStatus = ResponseStatus.Completed,
            IsSuccessStatusCode = true
        };

        var client = new Mock<IAiHttpClient>();
        client.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>()))
              .ReturnsAsync(response);

        var service = new AiService(config.Object, client.Object);

        var result = await service.AskAsync("hello");

        Assert.StartsWith("Error parsing response:", result);
    }
}

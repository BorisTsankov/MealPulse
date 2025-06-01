using Xunit;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Services.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using DTOs.DTOs;

public class OpenFoodFactsServiceTests
{
    private HttpClient CreateMockClient(HttpStatusCode statusCode, string json)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(json),
            });

        return new HttpClient(handlerMock.Object);
    }

    [Fact]
    public async Task GetFoodItemByBarcodeAsync_ValidBarcode_ReturnsDto()
    {
        string json = @"
        {
            ""status"": 1,
            ""product"": {
                ""product_name"": ""Banana"",
                ""brands"": ""Dole"",
                ""generic_name"": ""Fruit"",
                ""image_front_url"": ""http://example.com/banana.jpg"",
                ""nutriments"": {
                    ""energy-kcal_100g"": 89,
                    ""proteins_100g"": 1.1,
                    ""fat_100g"": 0.3,
                    ""carbohydrates_100g"": 22.8,
                    ""sugars_100g"": 12.2,
                    ""fiber_100g"": 2.6,
                    ""sodium_100g"": 0.001,
                    ""potassium_100g"": 358,
                    ""iron_100g"": 0.26,
                    ""calcium_100g"": 5
                }
            }
        }";

        var client = CreateMockClient(HttpStatusCode.OK, json);
        var service = new OpenFoodFactsService(client);

        var result = await service.GetFoodItemByBarcodeAsync("123456789");

        Assert.NotNull(result);
        Assert.Equal("Banana", result!.Name);
        Assert.Equal("Dole", result.Brand);
        Assert.Equal(89, result.Calories);
    }

    [Fact]
    public async Task GetFoodItemByBarcodeAsync_StatusNotOne_ReturnsNull()
    {
        string json = @"{ ""status"": 0 }";
        var client = CreateMockClient(HttpStatusCode.OK, json);
        var service = new OpenFoodFactsService(client);

        var result = await service.GetFoodItemByBarcodeAsync("unknown");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetFoodItemByBarcodeAsync_ResponseNotSuccess_ReturnsNull()
    {
        var client = CreateMockClient(HttpStatusCode.NotFound, "");
        var service = new OpenFoodFactsService(client);

        var result = await service.GetFoodItemByBarcodeAsync("badcode");

        Assert.Null(result);
    }

    [Fact]
    public async Task SearchByNameAsync_ValidResponse_ReturnsList()
    {
        string json = @"
        {
            ""products"": [
                {
                    ""product_name"": ""Apple"",
                    ""brands"": ""Nature"",
                    ""generic_name"": ""Fruit"",
                    ""image_front_url"": ""http://example.com/apple.jpg"",
                    ""code"": ""123"",
                    ""nutriments"": {
                        ""energy-kcal_100g"": 52,
                        ""proteins_100g"": 0.3,
                        ""fat_100g"": 0.2,
                        ""carbohydrates_100g"": 14,
                        ""sugars_100g"": 10,
                        ""fiber_100g"": 2.4,
                        ""sodium_100g"": 0.001
                    }
                }
            ]
        }";

        var client = CreateMockClient(HttpStatusCode.OK, json);
        var service = new OpenFoodFactsService(client);

        var result = await service.SearchByNameAsync("apple");

        Assert.Single(result);
        Assert.Equal("Apple", result[0].Name);
        Assert.Equal(52, result[0].Calories);
    }

    [Fact]
    public async Task SearchByNameAsync_InvalidResponse_ReturnsEmpty()
    {
        var client = CreateMockClient(HttpStatusCode.InternalServerError, "");
        var service = new OpenFoodFactsService(client);

        var result = await service.SearchByNameAsync("apple");

        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchByNameAsync_MissingNutrients_SkipsItem()
    {
        string json = @"{ ""products"": [ { ""product_name"": ""Mystery"" } ] }";

        var client = CreateMockClient(HttpStatusCode.OK, json);
        var service = new OpenFoodFactsService(client);

        var result = await service.SearchByNameAsync("mystery");

        Assert.Empty(result);
    }
}

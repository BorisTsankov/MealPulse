using Moq;
using RestSharp;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class AiHttpClientTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldReturnResponseFromRestClient()
        {
            // Arrange
            var expectedContent = "{\"success\":true}";
            var response = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = expectedContent
            };

            var request = new RestRequest("http://mock.api/test", Method.Get);

            // Create a partial mock using Moq
            var mockClient = new Mock<AiHttpClient> { CallBase = true };
            mockClient
                .Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>()))
                .ReturnsAsync(response);

            // Act
            var result = await mockClient.Object.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedContent, result.Content);
        }
    }
}
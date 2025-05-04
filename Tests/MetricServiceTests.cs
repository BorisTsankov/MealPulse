using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Moq;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class MetricServiceTests
    {
        private readonly Mock<IMetricRepository> _metricRepoMock;
        private readonly MetricService _metricService;

        public MetricServiceTests()
        {
            _metricRepoMock = new Mock<IMetricRepository>();
            _metricService = new MetricService(_metricRepoMock.Object);
        }

        [Fact]
        public void GetAllMetrics_ShouldReturnList_WhenCalled()
        {
            // Arrange
            var expected = new List<Metric>
        {
            new Metric { MetricId = 1, MetricName = "Metric 1" },
            new Metric { MetricId = 2, MetricName = "Metric 2" }
        };

            _metricRepoMock.Setup(r => r.GetAll()).Returns(expected);

            // Act
            var result = _metricService.GetAllMetrics();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Metric 1", result[0].MetricName);
        }

        [Fact]
        public void GetMetricById_ShouldReturnMetric_WhenExists()
        {
            // Arrange
            var metric = new Metric { MetricId = 1, MetricName = "Metric A" };
            _metricRepoMock.Setup(r => r.GetById(1)).Returns(metric);

            // Act
            var result = _metricService.GetMetricById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Metric A", result!.MetricName);
        }

        [Fact]
        public void GetMetricById_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _metricRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((Metric?)null);

            // Act
            var result = _metricService.GetMetricById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetMetricNameById_ShouldReturnCorrectName()
        {
            // Arrange
            _metricRepoMock.Setup(r => r.GetMetricNameById(1)).Returns("Grams");

            // Act
            var name = _metricService.GetMetricNameById(1);

            // Assert
            Assert.Equal("Grams", name);
        }

        [Fact]
        public void GetMetricNameById_ShouldReturnEmpty_WhenUnknownId()
        {
            // Arrange
            _metricRepoMock.Setup(r => r.GetMetricNameById(It.IsAny<int>())).Returns(string.Empty);

            // Act
            var name = _metricService.GetMetricNameById(999);

            // Assert
            Assert.Equal(string.Empty, name);
        }
    }
}

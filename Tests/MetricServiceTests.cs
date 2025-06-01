using DataAccess.Repositories.Interfaces;
using Models.Models;
using Moq;
using Services.Services;

public class MetricServiceTests
{
    private readonly Mock<IMetricRepository> _metricRepositoryMock;
    private readonly MetricService _metricService;

    public MetricServiceTests()
    {
        _metricRepositoryMock = new Mock<IMetricRepository>();
        _metricService = new MetricService(_metricRepositoryMock.Object);
    }

    // ---------------------
    // GetAllMetrics Tests
    // ---------------------

    [Fact]
    public void GetAllMetrics_ReturnsAllMetrics()
    {
        var metrics = new List<Metric>
        {
            new Metric { MetricId = 1, MetricName = "Kilogram" },
            new Metric { MetricId = 2, MetricName = "Pound" }
        };

        _metricRepositoryMock.Setup(repo => repo.GetAll()).Returns(metrics);

        var result = _metricService.GetAllMetrics();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, m => m.MetricName == "Kilogram");
        Assert.Contains(result, m => m.MetricName == "Pound");
    }

    [Fact]
    public void GetAllMetrics_EmptyList_ReturnsEmptyList()
    {
        _metricRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Metric>());

        var result = _metricService.GetAllMetrics();

        Assert.Empty(result);
    }

    [Fact]
    public void GetAllMetrics_RepositoryReturnsNull_Throws()
    {
        _metricRepositoryMock.Setup(repo => repo.GetAll()).Returns((List<Metric>)null!);

        Assert.Throws<System.NullReferenceException>(() => _metricService.GetAllMetrics());
    }

    // ---------------------
    // GetMetricById Tests
    // ---------------------

    [Fact]
    public void GetMetricById_ValidId_ReturnsMetric()
    {
        var metric = new Metric {MetricId = 1, MetricName = "Kilogram" };
        _metricRepositoryMock.Setup(repo => repo.GetById(1)).Returns(metric);

        var result = _metricService.GetMetricById(1);

        Assert.NotNull(result);
        Assert.Equal("Kilogram", result!.MetricName);
    }

    [Fact]
    public void GetMetricById_InvalidId_ReturnsNull()
    {
        _metricRepositoryMock.Setup(repo => repo.GetById(999)).Returns((Metric?)null);

        var result = _metricService.GetMetricById(999);

        Assert.Null(result);
    }

    [Fact]
    public void GetMetricById_NegativeId_ReturnsNull()
    {
        _metricRepositoryMock.Setup(repo => repo.GetById(-5)).Returns((Metric?)null);

        var result = _metricService.GetMetricById(-5);

        Assert.Null(result);
    }

    // ---------------------
    // GetMetricNameById Tests
    // ---------------------

    [Fact]
    public void GetMetricNameById_ValidId_ReturnsName()
    {
        _metricRepositoryMock.Setup(repo => repo.GetMetricNameById(1)).Returns("Gram");

        var result = _metricService.GetMetricNameById(1);

        Assert.Equal("Gram", result);
    }

    [Fact]
    public void GetMetricNameById_InvalidId_ReturnsNullOrEmpty()
    {
        _metricRepositoryMock.Setup(repo => repo.GetMetricNameById(999)).Returns((string?)null);

        var result = _metricService.GetMetricNameById(999);

        Assert.Null(result);
    }

    [Fact]
    public void GetMetricNameById_NegativeId_ReturnsNull()
    {
        _metricRepositoryMock.Setup(repo => repo.GetMetricNameById(-3)).Returns((string?)null);

        var result = _metricService.GetMetricNameById(-3);

        Assert.Null(result);
    }
}

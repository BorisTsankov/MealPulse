using Models.Models;

namespace Services.Services.Interfaces
{
    public interface IMetricService
    {
        List<Metric> GetAllMetrics();
        Metric? GetMetricById(int id);
        string GetMetricNameById(int id);

    }
}

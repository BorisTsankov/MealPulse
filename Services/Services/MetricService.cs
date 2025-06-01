using DataAccess.Repositories.Interfaces;
using Models.Models;
using Services.Services.Interfaces;

namespace Services.Services
{
    public class MetricService : IMetricService
    {
        private readonly IMetricRepository _metricRepository;

        public MetricService(IMetricRepository metricRepository)
        {
            _metricRepository = metricRepository;
        }

        public List<Metric> GetAllMetrics()
        {
            var result = _metricRepository.GetAll();
            if (result == null)
                throw new NullReferenceException("Metric repository returned null");

            return result;
        }

        public Metric? GetMetricById(int id)
        {
            return _metricRepository.GetById(id);
        }

        public string GetMetricNameById(int id)
        {
            return _metricRepository.GetMetricNameById(id);
        }
    }
}

using DataAccess.Repositories.Interfaces;
using MealPulse.Models.Models;
using Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return _metricRepository.GetAll();
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

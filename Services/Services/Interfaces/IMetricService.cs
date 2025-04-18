using MealPulse.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Interfaces
{
    public interface IMetricService
    {
        List<Metric> GetAllMetrics();
        Metric? GetMetricById(int id);
        string GetMetricNameById(int id);

    }
}

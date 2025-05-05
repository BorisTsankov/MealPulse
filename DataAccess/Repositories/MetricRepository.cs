using DataAccess.Data;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess.Repositories
{
    public class MetricRepository : IMetricRepository
    {
        private readonly DbHelper _dbHelper;

        public MetricRepository(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<Metric> GetAll()
        {
            string query = "SELECT * FROM Metric";
            var dt = _dbHelper.ExecuteQuery(query);

            var result = new List<Metric>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new Metric
                {
                    MetricId = Convert.ToInt32(row["metric_id"]),
                    MetricName = row["metric"].ToString()!
                });
            }

            return result;
        }

        public Metric? GetById(int id)
        {
            string query = "SELECT * FROM Metric WHERE metric_id = @metric_id";
            var parameters = new Dictionary<string, object> { { "@metric_id", id } };
            var dt = _dbHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            return new Metric
            {
                MetricId = Convert.ToInt32(row["metric_id"]),
                MetricName = row["metric"].ToString()!
            };
        }
        public string GetMetricNameById(int id)
        {
            return GetById(id)?.MetricName ?? "Unknown";
        }

    }
}

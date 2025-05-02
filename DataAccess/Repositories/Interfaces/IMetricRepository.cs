using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IMetricRepository
    {
        List<Metric> GetAll();
        Metric? GetById(int id);
        string GetMetricNameById(int id);

    }

}

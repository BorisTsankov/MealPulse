using Models.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IMetricRepository
    {
        List<Metric> GetAll();
        Metric? GetById(int id);
        string GetMetricNameById(int id);

    }

}

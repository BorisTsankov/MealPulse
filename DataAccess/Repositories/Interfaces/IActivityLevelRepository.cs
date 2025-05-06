using Models.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IActivityLevelRepository
    {
        List<ActivityLevel> GetAll();
        ActivityLevel? GetById(int id);
        string GetActivityLevelNameById(int id);
    }
}

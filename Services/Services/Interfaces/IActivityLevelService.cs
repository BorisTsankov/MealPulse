using Models.Models;

namespace Services.Services.Interfaces
{
    public interface IActivityLevelService
    {
        List<ActivityLevel> GetAll();
        string GetActivityLevelName(int id);
    }
}

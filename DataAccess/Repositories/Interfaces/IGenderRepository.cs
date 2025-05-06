using Models.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IGenderRepository
    {
        List<Gender> GetAll();
        Gender? GetById(int id);
        string GetGenderNameById(int id);
    }
}

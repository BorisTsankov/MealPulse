using Models.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IMealTypeRepository
    {
        List<MealType> GetAll();
    }
}

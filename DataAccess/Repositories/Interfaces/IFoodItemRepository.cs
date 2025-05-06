using Models.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IFoodItemRepository
    {
        List<FoodItem> GetAll();
        FoodItem? GetById(int id);
        List<FoodItem> SearchByName(string name);

    }
}

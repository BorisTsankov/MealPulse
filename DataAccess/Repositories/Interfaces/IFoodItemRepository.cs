using Models.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IFoodItemRepository
    {
        List<FoodItem> GetAll();
        FoodItem GetByBarcode(string barcode);
        FoodItem? GetById(int id);
        List<FoodItem> SearchByName(string name);
        int Add(FoodItem item); 


    }
}

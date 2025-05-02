using DataAccess.Models;

namespace MealPulse.Services.Interfaces
{
    public interface IFoodItemService
    {
        int InsertTestFood();
        List<FoodItem> GetAll();
    }
}

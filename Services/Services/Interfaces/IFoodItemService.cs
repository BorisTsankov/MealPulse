using DataAccess.Models;
using Services.Models;

namespace Services.Services.Interfaces
{
    public interface IFoodItemService
    {
        List<FoodItemDto> GetAll();
        FoodItemDto? GetById(int id);
        List<FoodItemDto> SearchByName(string name);

    }
}

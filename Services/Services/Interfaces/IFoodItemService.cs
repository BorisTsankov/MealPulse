using DTOs.DTOs;

namespace Services.Services.Interfaces
{
    public interface IFoodItemService
    {
        List<FoodItemDto> GetAll();
        FoodItemDto? GetById(int id);
        List<FoodItemDto> SearchByName(string name);

    }
}

using DTOs.DTOs;

namespace Services.Services.Interfaces
{
    public interface IFoodItemService
    {
        List<FoodItemDto> GetAll();
        FoodItemDto? GetById(int id);
        List<FoodItemDto> SearchByName(string name);
        Task<FoodItemDto?> GetByBarcodeOrFetchAsync(string barcode);
        Task<List<FoodItemDto>> SearchByNameOrFetchAsync(string term);


    }
}

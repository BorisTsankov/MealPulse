using DataAccess.Repositories.Interfaces;
using DTOs.DTOs;
using Services.Mappers;
using Services.Services.Interfaces;
using System.Data;

namespace Services.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IOpenFoodFactsService _openFoodFactsService;
        private readonly IFoodItemRepository _repo;


        public FoodItemService(IFoodItemRepository repo, IOpenFoodFactsService openFoodFactsService)
        {
            _repo = repo;
            _openFoodFactsService = openFoodFactsService;
        }

        public List<FoodItemDto> GetAll()
        {
            var items = _repo.GetAll();
            return items.Select(FoodItemMapper.ToDto).ToList();
        }

        public FoodItemDto? GetById(int id)
        {
            var item = _repo.GetById(id);
            return item != null ? FoodItemMapper.ToDto(item) : null;
        }

        public List<FoodItemDto> SearchByName(string name)
        {
            var items = _repo.SearchByName(name);
            return items.Select(FoodItemMapper.ToDto).ToList();
        }

        public async Task<FoodItemDto?> GetByBarcodeOrFetchAsync(string barcode)
        {
            var local = _repo.GetByBarcode(barcode);
            if (local != null)
                return FoodItemMapper.ToDto(local);

            var external = await _openFoodFactsService.GetFoodItemByBarcodeAsync(barcode);
            if (external != null)
            {
                var entity = FoodItemMapper.ToEntity(external);
                var newId = _repo.Add(entity); // ✅ Save and get ID
                external.FoodItemId = newId;     // ✅ Set ID for later use

                return external;
            }


            return null;
        }

        public async Task<List<FoodItemDto>> SearchByNameOrFetchAsync(string term)
        {
            var localMatches = _repo.SearchByName(term);
            if (localMatches.Any())
                return localMatches.Select(FoodItemMapper.ToDto).ToList();

            var apiResult = await _openFoodFactsService.SearchByNameAsync(term);
            if (apiResult != null && apiResult.Any())
            {
                var validItems = new List<FoodItemDto>();

                foreach (var item in apiResult)
                {
                    // ✅ Smarter filter: allow water etc., skip fully empty
                    if (string.IsNullOrWhiteSpace(item.Name) ||
                        (item.Calories == 0 && item.Protein == 0 && item.Fat == 0 && item.Carbohydrates == 0))
                        continue;

                    var existing = !string.IsNullOrWhiteSpace(item.Barcode)
                        ? _repo.GetByBarcode(item.Barcode)
                        : _repo.SearchByName(item.Name).FirstOrDefault();

                    if (existing == null)
                    {
                        var entity = FoodItemMapper.ToEntity(item);
                        int newId = _repo.Add(entity);
                        item.FoodItemId = newId;
                    }

                    validItems.Add(item); // ✅ Keep for returning
                }

                return validItems;
            }

            return new List<FoodItemDto>();
        }


        public FoodItemDto? GetByName(string name)
        {
            var match = _repo.SearchByName(name)
                .FirstOrDefault(f => f.Calories > 0); // ✅ Ensure not a dummy
            return match != null ? FoodItemMapper.ToDto(match) : null;
        }


        public int Add(FoodItemDto dto)
        {
            var entity = FoodItemMapper.ToEntity(dto);
            return _repo.Add(entity); // This is already implemented and returns the new ID
        }




    }
}
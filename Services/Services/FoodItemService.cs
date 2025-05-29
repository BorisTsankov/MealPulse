using DataAccess.Repositories.Interfaces;
using DTOs.DTOs;
using Services.Mappers;
using Services.Services.Interfaces;
using System.Data;

namespace Services.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly OpenFoodFactsService _openFoodFactsService;
        private readonly IFoodItemRepository _repo;

        public FoodItemService(IFoodItemRepository repo, OpenFoodFactsService openFoodFactsService)
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

            // ✅ If local results exist, return them
            if (localMatches.Any())
                return localMatches.Select(FoodItemMapper.ToDto).ToList();

            // ❌ Nothing found locally, fetch from API
            var apiResult = await _openFoodFactsService.SearchByNameAsync(term);
            if (apiResult != null && apiResult.Any())
            {
                foreach (var item in apiResult)
                {
                    // ⚠️ Skip incomplete items
                    if (item.Calories == 0 || item.Protein == 0 || item.Fat == 0)
                        continue;

                    // ✅ Check for duplicates by barcode or name
                    var existing = !string.IsNullOrWhiteSpace(item.Barcode)
                        ? _repo.GetByBarcode(item.Barcode)
                        : _repo.SearchByName(item.Name).FirstOrDefault();

                    if (existing == null)
                    {
                        // 🔥 INSERT AND GET ID BACK
                        var entity = FoodItemMapper.ToEntity(item);
                        int newId = _repo.Add(entity);
                        item.FoodItemId = newId;
                    }
                }

                return apiResult;
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
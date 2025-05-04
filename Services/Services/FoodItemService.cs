using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using MealPulse.Data;
using MealPulse.Services.Interfaces;
using Services.Mappers;
using Services.Models;
using System.Data;

namespace MealPulse.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IFoodItemRepository _repo;

        public FoodItemService(IFoodItemRepository repo)
        {
            _repo = repo;
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

    }
}
using DataAccess.Repositories.Interfaces;
using Models.Models;
using Services.Services.Interfaces;

namespace Services.Services
{
    public class MealTypeService : IMealTypeService
    {
        private readonly IMealTypeRepository _repo;

        public MealTypeService(IMealTypeRepository repo)
        {
            _repo = repo;
        }

        public List<MealType> GetAll()
        {
            var result = _repo.GetAll();
            if (result == null)
                throw new NullReferenceException("MealType repository returned null");

            return result;
        }
    }
}

using DataAccess.Repositories.Interfaces;
using Models.Models;
using Services.Services.Interfaces;

namespace Services.Services
{
    public class GenderService : IGenderService
    {
        private readonly IGenderRepository _genderRepository;

        public GenderService(IGenderRepository genderRepository)
        {
            _genderRepository = genderRepository;
        }

        public List<Gender> GetAll()
        {
            var result = _genderRepository.GetAll();
            if (result == null)
                throw new NullReferenceException("Gender repository returned null");

            return result;
        }

        public string GetGenderName(int id)
        {
            return _genderRepository.GetGenderNameById(id);
        }
    }
}

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
            return _genderRepository.GetAll();
        }

        public string GetGenderName(int id)
        {
            return _genderRepository.GetGenderNameById(id);
        }
    }

}

using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MealPulse.Services.Interfaces;
using Services.Services.Interfaces;
using MealPulse.Models.Models;

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

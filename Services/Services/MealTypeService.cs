using DataAccess.Repositories.Interfaces;
using MealPulse.Models.Models;
using Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return _repo.GetAll();
        }
    }

}

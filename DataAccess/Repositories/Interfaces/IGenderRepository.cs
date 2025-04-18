using MealPulse.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IGenderRepository
    {
        List<Gender> GetAll();
        Gender? GetById(int id);
        string GetGenderNameById(int id);
    }
}

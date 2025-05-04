using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IFoodItemRepository
    {
        List<FoodItem> GetAll();
        FoodItem? GetById(int id);
        List<FoodItem> SearchByName(string name);

    }
}

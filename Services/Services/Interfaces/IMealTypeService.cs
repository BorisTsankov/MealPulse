using MealPulse.Models.Models;
using System.Collections.Generic;

namespace Services.Services.Interfaces
{
    public interface IMealTypeService
    {
        List<MealType> GetAll();
    }
}
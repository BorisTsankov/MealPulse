using DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Interfaces
{
    public interface IOpenFoodFactsService
    {
        Task<FoodItemDto?> GetFoodItemByBarcodeAsync(string barcode);
        Task<List<FoodItemDto>> SearchByNameAsync(string term);
    }
}
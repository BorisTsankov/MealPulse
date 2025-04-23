using System;
using System.Collections.Generic;

namespace Core.Models.ViewModels
{
    public class FoodDiaryViewModel
    {
        public List<FoodDiarySectionViewModel> Sections { get; set; } = new();
    }

    public class FoodDiarySectionViewModel
    {
        public string MealName { get; set; } = null!;
        public int MealTypeId { get; set; }
        public List<FoodDiaryItemViewModel> Items { get; set; } = new();
        public decimal TotalCalories { get; set; }
    }

    public class FoodDiaryItemViewModel
    {
        public string FoodName { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "g";
        public decimal Calories { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class FoodDiaryItemDto
    {
        public int FoodDiaryItemId { get; set; }
        public int GoalId { get; set; }
        public int FoodId { get; set; }
        public int MealTypeId { get; set; }
        public DateTime DateTime { get; set; }
        public double Quantity { get; set; }

        // Optional: Names from navigation properties if needed
        public string? FoodName { get; set; }
        public string? MealTypeName { get; set; }
    }
}
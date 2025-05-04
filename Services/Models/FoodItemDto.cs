using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class FoodItemDto
    {
        public int? FoodItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = "g";

        // Nutritional info
        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Sugars { get; set; }
        public decimal Fiber { get; set; }
        public decimal Sodium { get; set; }
        public decimal Potassium { get; set; }
        public decimal Iron { get; set; }
        public decimal Calcium { get; set; }
    }
}

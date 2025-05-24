using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.DTOs
{
    public class ChatFoodLog
    {
        public string foodName { get; set; }
        public double quantity { get; set; }
        public string unit { get; set; }
        public string mealType { get; set; }

        public decimal calories { get; set; }
        public decimal protein { get; set; }
        public decimal fat { get; set; }
        public decimal carbohydrates { get; set; }
        public decimal sugars { get; set; }
        public decimal fiber { get; set; }
        public decimal sodium { get; set; }
        public decimal potassium { get; set; }
        public decimal iron { get; set; }
        public decimal calcium { get; set; }
    }


}
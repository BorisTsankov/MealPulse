using MealPulse.Data;
using Microsoft.AspNetCore.Mvc;

namespace MealPulse.Controllers
{
    public class FoodItemController : Controller
    {
        private readonly DbHelper _dbHelper;

        public FoodItemController(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        [HttpGet]
        public IActionResult InsertTestFood()
        {
            var query = @"INSERT INTO FoodItem (name, brand, serving_g, calories_per_100g, calories_per_100ml, protein_g, fats_g, carbs_g)
                          VALUES (@name, @brand, @serving_g, @calories_per_100g, @calories_per_100ml, protein_g, fats_g, carbs_g)";

            var parameters = new Dictionary<string, object>
            {
                { "@name", "Banana" },
                { "@brand", ""},
                { "@serving_g", 1 },
                { "@calories_per_100g", 22.8 },
                { "@calories_per_100ml", 0.3 },
                { "@protein_g", 0.3 },
                { "@fats_g", 0.3 },
                { "@carbs_g", 0.3 }

            };

            int rows = _dbHelper.ExecuteNonQuery(query, parameters);

            return Content(rows > 0 ? "Food inserted!" : "Insert failed.");
        }
    }
}

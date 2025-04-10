using MealPulse.Data;
using MealPulse.Services.Interfaces;

namespace MealPulse.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly DbHelper _dbHelper;

        public FoodItemService(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public int InsertTestFood()
        {
            var query = @"
                INSERT INTO FoodItem (name, brand, serving_g, calories_per_100g, calories_per_100ml, protein_g, fats_g, carbs_g)
                VALUES (@name, @brand, @serving_g, @calories_per_100g, @calories_per_100ml, @protein_g, @fats_g, @carbs_g)
            ";

            var parameters = new Dictionary<string, object>
            {
                { "@name", "Banana" },
                { "@brand", "" },
                { "@serving_g", 1 },
                { "@calories_per_100g", 22.8 },
                { "@calories_per_100ml", 0.3 },
                { "@protein_g", 0.3 },
                { "@fats_g", 0.3 },
                { "@carbs_g", 0.3 }
            };

            return _dbHelper.ExecuteNonQuery(query, parameters);
        }
    }
}

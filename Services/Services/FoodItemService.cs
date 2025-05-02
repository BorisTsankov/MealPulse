using DataAccess.Models;
using MealPulse.Data;
using MealPulse.Services.Interfaces;
using System.Data;

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
        public List<FoodItem> GetAll()
        {
            var query = "SELECT * FROM FoodItem";
            var dt = _dbHelper.ExecuteQuery(query);

            var items = new List<FoodItem>();
            foreach (DataRow row in dt.Rows)
            {
                items.Add(new FoodItem
                {
                    FoodItemId = Convert.ToInt32(row["FoodItemId"]),
                    Name = row["Name"].ToString()!,
                    Calories = Convert.ToDecimal(row["Calories"]),
                    Protein = Convert.ToDecimal(row["Protein"]),
                    Fat = Convert.ToDecimal(row["Fat"]),
                    Carbohydrates = Convert.ToDecimal(row["Carbohydrates"]),
                    Sugars = Convert.ToDecimal(row["Sugars"]),
                    Fiber = Convert.ToDecimal(row["Fiber"]),
                    Sodium = row["Sodium"] == DBNull.Value ? null : Convert.ToDecimal(row["Sodium"]),
                    Potassium = row["Potassium"] == DBNull.Value ? null : Convert.ToDecimal(row["Potassium"]),
                    Iron = row["Iron"] == DBNull.Value ? null : Convert.ToDecimal(row["Iron"]),
                    Calcium = row["Calcium"] == DBNull.Value ? null : Convert.ToDecimal(row["Calcium"]),
                    Unit = row["Unit"].ToString() ?? "g"
                });
            }

            return items;
        }
    }
}

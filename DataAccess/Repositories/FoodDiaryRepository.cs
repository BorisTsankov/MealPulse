using MealPulse.Models.Models;
using DataAccess.Repositories.Interfaces;
using MealPulse.Data;
using System.Collections.Generic;
using System.Data;

namespace DataAccess.Repositories
{
    public class FoodDiaryRepository : IFoodDiaryRepository
    {
        private readonly DbHelper _dbHelper;

        public FoodDiaryRepository(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<FoodDiaryItem> GetItemsByGoalId(int goalId)
        {
            string query = @"
                SELECT fdi.*, fi.name AS FoodItemName, fi.calories, fi.unit
                FROM FoodDiaryItem fdi
                JOIN FoodItem fi ON fi.foodItemId = fdi.food_id
                WHERE fdi.goal_id = @GoalId";

            var parameters = new Dictionary<string, object>
            {
                { "@GoalId", goalId }
            };

            var dt = _dbHelper.ExecuteQuery(query, parameters);
            var items = new List<FoodDiaryItem>();

            foreach (DataRow row in dt.Rows)
            {
                items.Add(new FoodDiaryItem
                {
                    FoodDiaryItemId = (int)row["FoodDiaryItem_id"],
                    MealTypeId = (int)row["mealType_id"],
                    GoalId = (int)row["goal_id"],
                    FoodId = (int)row["food_id"],
                    Quantity = (double)(decimal)row["quantity"],
                    DateTime = (DateTime)row["date_time"],

                    FoodItem = new FoodItem // ✅ This is the missing part
                    {
                        Name = row["FoodItemName"].ToString()!,
                        Calories = Convert.ToDecimal(row["calories"]),
                        Unit = row["unit"].ToString()!
                    }
                });
              }

            return items;
        }

        public bool Add(FoodDiaryItem item)
        {
            var query = @"
            INSERT INTO FoodDiaryItem 
            (goal_id, food_id, mealType_id, date_time, quantity)
            VALUES 
            (@GoalId, @FoodId, @MealTypeId, @DateTime, @Quantity)";

            var parameters = new Dictionary<string, object>
        {
            { "@GoalId", item.GoalId },
            { "@FoodId", item.FoodId },
            { "@MealTypeId", item.MealTypeId },
            { "@DateTime", item.DateTime },
            { "@Quantity", item.Quantity }
        };

            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }


    }
}

using DataAccess.Data;
using DataAccess.Repositories.Interfaces;
using Models.Models;
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

                    FoodItem = new FoodItem
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
        public List<FoodDiaryItem> GetItemsByGoalIdAndDate(int goalId, DateTime date)
        {
            string query = @"
          SELECT fdi.*, fi.name AS FoodItemName, fi.calories, fi.unit
        FROM FoodDiaryItem fdi
        JOIN FoodItem fi ON fi.foodItemId = fdi.food_id
        WHERE fdi.goal_id = @GoalId
        AND CAST(fdi.date_time AS DATE) = @Date";

            var parameters = new Dictionary<string, object>
    {
        { "@GoalId", goalId },
        { "@Date", date.Date }
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
                    FoodItem = new FoodItem
                    {
                        Name = row["FoodItemName"].ToString()!,
                        Calories = Convert.ToDecimal(row["calories"]),
                        Unit = row["unit"].ToString()!
                    }
                });
            }

            return items;
        }

        public bool Delete(int foodDiaryItemId)
        {
            string query = "DELETE FROM FoodDiaryItem WHERE FoodDiaryItem_id = @Id";
            var parameters = new Dictionary<string, object> { { "@Id", foodDiaryItemId } };
            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public List<FoodDiaryItem> GetItemsByUserIdAndDate(int userId, DateTime date)
        {
            string query = @"
        SELECT fdi.*, fi.name AS FoodItemName, fi.calories, fi.unit
        FROM FoodDiaryItem fdi
        JOIN Goal g ON fdi.goal_id = g.goal_id
        JOIN FoodItem fi ON fi.foodItemId = fdi.food_id
        WHERE g.user_id = @UserId
        AND CAST(fdi.date_time AS DATE) = @Date";

            var parameters = new Dictionary<string, object>
    {
        { "@UserId", userId },
        { "@Date", date.Date }
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
                    FoodItem = new FoodItem
                    {
                        Name = row["FoodItemName"].ToString()!,
                        Calories = Convert.ToDecimal(row["calories"]),
                        Unit = row["unit"].ToString()!
                    }
                });
            }

            return items;
        }

    }
}
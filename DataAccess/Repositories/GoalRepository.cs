using DataAccess.Repositories.Interfaces;
using MealPulse.Data;
using MealPulse.Data;
using MealPulse.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class GoalRepository : IGoalRepository
    {
        private readonly DbHelper _db;


        public GoalRepository(DbHelper db)
        {
            _db = db;
        }

        public Goal? GetMostRecentGoalByUserId(int userId)
        {
            string sql = @"
        SELECT TOP 1 goal_id, user_id, target_weight_kg, current_weight_kg, start_date
        FROM [Goal]
        WHERE user_id = @user_id
        ORDER BY start_date DESC";

            var parameters = new Dictionary<string, object> { { "@user_id", userId } };
            var dt = _db.ExecuteQuery(sql, parameters);

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return new Goal
            {
                goal_id = (int)row["goal_id"],
                user_id = (int)row["user_id"],
                target_weight_kg = (decimal)row["target_weight_kg"],
                current_weight_kg = (decimal)row["current_weight_kg"],
                start_date = (DateTime)row["start_date"]
            };
        }

        public bool UpdateWeight(int userId, decimal newWeight)
        {
            var parameters = new Dictionary<string, object>
    {
        { "@user_id", userId },
        { "@newWeight", newWeight }
    };

            string deactivateSql = @"
        UPDATE [Goal]
        SET is_active = 'false'
        WHERE user_id = @user_id AND is_active = 'true'";
            _db.ExecuteNonQuery(deactivateSql, parameters);

            string insertSql = @"
        INSERT INTO [Goal] (user_id, current_weight_kg, target_weight_kg, start_date, is_active)
        VALUES (@user_id, @newWeight, 0, GETDATE(), 'true')";
            return _db.ExecuteNonQuery(insertSql, parameters) > 0;
        }

    }
}
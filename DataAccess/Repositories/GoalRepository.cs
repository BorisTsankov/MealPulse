using Core.Models.Enums;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using MealPulse.Data;
using System;
using System.Collections.Generic;

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
        SELECT TOP 1 goal_id, user_id, target_weight_kg, current_weight_kg, start_date, goal_intensity
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
                start_date = (DateTime)row["start_date"],
                goal_intensity = row["goal_intensity"] == DBNull.Value ? (int)GoalIntensity.Maintain : Convert.ToInt32(row["goal_intensity"]) // Handle DBNull and cast properly
            };
        }

        public bool UpdateWeight(int userId, decimal newWeight)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@user_id", userId },
                { "@newWeight", newWeight }
            };

            // Deactivate the current active goal and set end_date
            string deactivateSql = @"
        UPDATE [Goal]
        SET is_active = 0, end_date = GETDATE()
        WHERE user_id = @user_id AND is_active = 1";
            _db.ExecuteNonQuery(deactivateSql, parameters);

            // Create a new goal with the new weight and the same target weight and goal intensity
            Goal? mostRecentGoal = GetMostRecentGoalByUserId(userId);
            if (mostRecentGoal == null) return false;

            string insertSql = @"
        INSERT INTO [Goal] (user_id, current_weight_kg, target_weight_kg, start_date, is_active, goal_intensity)
        VALUES (@user_id, @newWeight, @target_weight, GETDATE(), 1, @goal_intensity)";

            var insertParameters = new Dictionary<string, object>
            {
                { "@user_id", userId },
                { "@newWeight", newWeight },
                { "@target_weight", mostRecentGoal.target_weight_kg }, // Keep the same target weight
                { "@goal_intensity", mostRecentGoal.goal_intensity }
            };

            return _db.ExecuteNonQuery(insertSql, insertParameters) > 0;
        }

        public bool CreateGoal(Goal goal)
        {
            var parameters = new Dictionary<string, object>
    {
        { "@user_id", goal.user_id },
        { "@current_weight", goal.current_weight_kg },
        { "@target_weight", goal.target_weight_kg },
        { "@goal_intensity", (int)goal.goal_intensity }
    };

            string deactivateSql = @"
        UPDATE [Goal]
        SET is_active = 0, end_date = GETDATE()
        WHERE user_id = @user_id AND is_active = 1";

            _db.ExecuteNonQuery(deactivateSql, parameters);

            string insertSql = @"
        INSERT INTO [Goal] (user_id, current_weight_kg, target_weight_kg, start_date, is_active, goal_intensity)
        VALUES (@user_id, @current_weight, @target_weight, GETDATE(), 1, @goal_intensity)";

            return _db.ExecuteNonQuery(insertSql, parameters) > 0;
        }

    }
}

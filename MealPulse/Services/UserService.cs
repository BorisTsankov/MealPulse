using MealPulse.Data;
using MealPulse.Models.Models;
using MealPulse.Services;
using System.Collections.Generic;
using System.Data;
using MealPulse.Services.Interfaces;

namespace MealPulse.Services
{
    public class UserService : IUserService
    {
        private readonly DbHelper _db;

        public UserService(DbHelper dbHelper)
        {
            _db = dbHelper;
        }

        public User? GetUserById(int userId)
        {
            string sql = @"
                SELECT user_id, FirstName, LastName, email, age, gender_id, height_cm, activityLevel_id, metric_id
                FROM [User]
                WHERE user_id = @user_id";

            var parameters = new Dictionary<string, object> { { "@user_id", userId } };
            var dt = _db.ExecuteQuery(sql, parameters);

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return new User
            {
                user_id = (int)row["user_id"],
                FirstName = row["FirstName"].ToString()!,
                LastName = row["LastName"].ToString()!,
                email = row["email"].ToString()!,
                age = (int)row["age"],
                gender_id = (int)row["gender_id"],
                height_cm = (decimal)row["height_cm"],
                activityLevel_id = (int)row["activityLevel_id"],
                metric_id = (int)row["metric_id"]
            };
        }

        public bool UpdateHeight(int userId, decimal newHeight)
        {
            string sql = "UPDATE [User] SET height_cm = @newHeight WHERE user_id = @user_id";
            var parameters = new Dictionary<string, object>
            {
                {"@user_id", userId},
                {"@newHeight", newHeight}
            };
            return _db.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool UpdateWeight(int userId, decimal newWeight)
        {
            string sql = "UPDATE [User] SET height_cm = @newWeight WHERE user_id = @user_id"; // likely should be weight_kg
            var parameters = new Dictionary<string, object>
            {
                {"@user_id", userId},
                {"@newWeight", newWeight}
            };
            return _db.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}

using MealPulse.Data.Interfaces;
using MealPulse.Models.Models;
using System.Data;

namespace MealPulse.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbHelper _db;

        public UserRepository(DbHelper db)
        {
            _db = db;
        }

        public User? GetUserById(int userId)
        {
            string sql = @"
                SELECT user_id, FirstName, LastName, email, date_of_birth, gender_id, height_cm, activityLevel_id, metric_id
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
                date_of_birth = (DateTime)row["date_of_birth"],
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

        public void UpdateActivityLevel(int userId, int activityLevelId)
        {
            string query = "UPDATE [User] SET activityLevel_id = @ActivityLevelId WHERE user_id = @UserId";
            var parameters = new Dictionary<string, object>
    {
        {"@ActivityLevelId", activityLevelId},
        {"@UserId", userId}
    };
            _db.ExecuteNonQuery(query, parameters);
        }




    }
}

using MealPulse.Data;
using MealPulse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace MealPulse.Services
{
    public class AuthService : IAuthService
    {
        private readonly DbHelper _dbHelper;

        public AuthService(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool UserExists(string email)
        {
            var parameters = new Dictionary<string, object> { { "@email", email } };
            string sql = "SELECT COUNT(*) FROM [User] WHERE email = @email";
            DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);
            return dt.Rows.Count > 0 && (int)dt.Rows[0][0] > 0;
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public int RegisterUser(Dictionary<string, object> parameters)
        {
            string sql = @"
                INSERT INTO [User] (FirstName, LastName, email, password, age, gender_id, height_cm, activityLevel_id, metric_id, role_id)
                VALUES (@FirstName, @LastName, @email, @password, @age, @gender_id, @height_cm, @activityLevel_id, @metric_id, @role_id)";
            return _dbHelper.ExecuteNonQuery(sql, parameters);
        }

        public DataTable AuthenticateUser(string email, string password)
        {
            string hashed = HashPassword(password);
            var parameters = new Dictionary<string, object>
            {
                { "@email", email },
                { "@password", hashed }
            };
            string sql = "SELECT user_id, email FROM [User] WHERE email = @email AND password = @password";
            return _dbHelper.ExecuteQuery(sql, parameters);
        }

        public List<SelectListItem> GetSelectListData(string tableName, string valueField, string textField)
        {
            string sql = $"SELECT {valueField}, {textField} FROM {tableName}";
            DataTable dt = _dbHelper.ExecuteQuery(sql);
            return dt.AsEnumerable()
                     .Select(row => new SelectListItem
                     {
                         Value = row[valueField].ToString(),
                         Text = row[textField].ToString()
                     })
                     .ToList();
        }
    }
}

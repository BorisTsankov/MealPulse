using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MealPulse.Data;
using System.Collections.Generic;
using MealPulse.Models.Models;
using System.Data;

namespace MealPulse.Controllers
{
    [Authorize] // Only allow authenticated users
    public class ProfileController : Controller
    {
        private readonly DbHelper _dbHelper;

        public ProfileController(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index()
        {
            // Get the user ID from the ClaimsPrincipal
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                // Handle the case where the user ID is not found or invalid
                return View("Error"); // Or redirect to an error page
            }

            // Fetch the user's profile data from the database
            string sql = @"
                SELECT user_id, FirstName, LastName, email, age, gender_id, height_cm, activityLevel_id, metric_id
                FROM [User]
                WHERE user_id = @user_id
            ";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@user_id", userId}
            };

            DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);

            if (dt.Rows.Count == 0)
            {
                // Handle the case where the user is not found
                return View("Error"); // Or redirect to an error page
            }

            // Map the data from the DataTable to a User object
            User user = new User
            {
                user_id = (int)dt.Rows[0]["user_id"],
                FirstName = dt.Rows[0]["FirstName"].ToString(),
                LastName = dt.Rows[0]["LastName"].ToString(),
                email = dt.Rows[0]["email"].ToString(),
                age = (int)dt.Rows[0]["age"],
                gender_id = (int)dt.Rows[0]["gender_id"],
                height_cm = (decimal)dt.Rows[0]["height_cm"],
                activityLevel_id = (int)dt.Rows[0]["activityLevel_id"],
                metric_id = (int)dt.Rows[0]["metric_id"]
            };

            // Pass the User object to the view
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateWeight(int user_id, decimal newWeight)
        {
            string sql = @"
        UPDATE [User] SET height_cm = @newWeight WHERE user_id = @user_id
    ";

            Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            {"@user_id", user_id},
            {"@newWeight", newWeight}
        };
            int rowsAffected = _dbHelper.ExecuteNonQuery(sql, parameters);

            if (rowsAffected > 0)
            {
                // Handle the case where the user is not found
                return RedirectToAction("Index", "Profile");
            }

            return RedirectToAction("Index", "Profile");
        }
        [HttpPost]
        public IActionResult UpdateHeight(int user_id, decimal newHeight)
        {
            string sql = @"
        UPDATE [User] SET height_cm = @newHeight WHERE user_id = @user_id
    ";

            Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            {"@user_id", user_id},
            {"@newHeight", newHeight}
        };
            int rowsAffected = _dbHelper.ExecuteNonQuery(sql, parameters);

            if (rowsAffected > 0)
            {
                // Handle the case where the user is not found
                return RedirectToAction("Index", "Profile");
            }

            return RedirectToAction("Index", "Profile");
        }
    }
}

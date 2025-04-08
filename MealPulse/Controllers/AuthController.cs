using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using MealPulse.Data;
using MealPulse.Models.Models;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

public class AuthController : Controller
{
    private readonly string _connectionString;
    private readonly DbHelper _dbHelper;

    public AuthController(IConfiguration config, DbHelper dbHelper)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
        _dbHelper = dbHelper;
    }

    public IActionResult Register()
    {
        // Pass select list data to the view
        ViewBag.GenderOptions = GetSelectListData("Gender", "gender_id", "gender");
        ViewBag.ActivityLevelOptions = GetSelectListData("ActivityLevel", "activityLevel_id", "activityLevel");
        ViewBag.MetricOptions = GetSelectListData("Metric", "metric_id", "metric");
        return View();
    }

    [HttpPost]
    public IActionResult Register(string FirstName, string LastName, string email, string password, int age, decimal height_cm, int gender_id, int activityLevel_id, int metric_id)
    {

        string passwordHash = HashPassword(password);

        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@FirstName", FirstName},
                {"@LastName", LastName},
                {"@email", email},
                {"@password", passwordHash},
                {"@age", age},
                {"@height_cm", height_cm},
                {"@role_id", 1}, // Default User Role.  Consider making this selectable.
                {"@gender_id", gender_id},
                {"@activityLevel_id", activityLevel_id},
                {"@metric_id",metric_id }
            };

        string sql = @"
            INSERT INTO [User] (FirstName, LastName, email, password, age, gender_id, height_cm, activityLevel_id, metric_id, role_id)
            VALUES (@FirstName, @LastName, @email, @password, @age, @gender_id, @height_cm, @activityLevel_id, @metric_id, @role_id)
        ";

        int rowsAffected = _dbHelper.ExecuteNonQuery(sql, parameters);

        if (rowsAffected > 0)
        {
            return RedirectToAction("Login"); // Or a "Registration Successful" page
        }
        else
        {
            ViewBag.ErrorMessage = "Registration failed. Please try again.";
            return View();
        }
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private List<SelectListItem> GetSelectListData(string tableName, string valueField, string textField)
    {
        string sql = $"SELECT {valueField}, {textField} FROM {tableName}";
        DataTable dt = _dbHelper.ExecuteQuery(sql);

        List<SelectListItem> selectList = new List<SelectListItem>();
        foreach (DataRow row in dt.Rows)
        {
            selectList.Add(new SelectListItem
            {
                Value = row[valueField].ToString(),
                Text = row[textField].ToString()
            });
        }
        return selectList;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        string passwordHash = HashPassword(password);

        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@email", email},
                {"@password", passwordHash}
            };

        string sql = @"
            SELECT COUNT(*) 
            FROM [User] 
            WHERE email = @email AND password = @password
        ";

        DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);

        if (dt.Rows.Count > 0 && (int)dt.Rows[0][0] > 0)
        {
            // Authentication successful
            // You might want to store some user data in session here
            return RedirectToAction("Index", "Home"); // Redirect to home page
        }
        else
        {
            // Authentication failed
            ViewBag.Error = "Invalid login attempt.";
            return View();
        }
    }

}

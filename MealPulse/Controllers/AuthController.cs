using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using System.Security.Claims;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ViewBag.GenderOptions = _authService.GetSelectListData("Gender", "gender_id", "gender");
            ViewBag.ActivityLevelOptions = _authService.GetSelectListData("ActivityLevel", "activityLevel_id", "activityLevel");
            ViewBag.MetricOptions = _authService.GetSelectListData("Metric", "metric_id", "metric");

            return View();
        }

        [HttpPost]
        public IActionResult Register(string FirstName, string LastName, string email, string password, DateTime date_of_birth, decimal height_cm, int gender_id, int activityLevel_id, int metric_id)
        {

            if (date_of_birth < new DateTime(1753, 1, 1) || date_of_birth > DateTime.Now)
            {
                ViewBag.ErrorMessage = "Please select a valid date of birth.";

                ViewBag.GenderOptions = _authService.GetSelectListData("Gender", "gender_id", "gender");
                ViewBag.ActivityLevelOptions = _authService.GetSelectListData("ActivityLevel", "activityLevel_id", "activityLevel");
                ViewBag.MetricOptions = _authService.GetSelectListData("Metric", "metric_id", "metric");

                return View();
            }

            if (_authService.UserExists(email))
            {
                ViewBag.ErrorMessage = "Email already registered. Please login.";
                return RedirectToAction("Login");
            }

            var parameters = new Dictionary<string, object>
    {
        { "@FirstName", FirstName },
        { "@LastName", LastName },
        { "@email", email },
        { "@password", _authService.HashPassword(password) },
        { "@date_of_birth", date_of_birth },
        { "@height_cm", height_cm },
        { "@role_id", 1 },
        { "@gender_id", gender_id },
        { "@activityLevel_id", activityLevel_id },
        { "@metric_id", metric_id }
    };

            if (_authService.RegisterUser(parameters) > 0)
                return RedirectToAction("Login");

            ViewBag.ErrorMessage = "Registration failed.";
            return View();
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _authService.AuthenticateUser(email, password);
            if (user.Rows.Count > 0)
            {
                var userId = Convert.ToInt32(user.Rows[0]["user_id"]);

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, user.Rows[0]["email"].ToString())
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // ✅ Store user_id in session
                HttpContext.Session.SetInt32("user_id", userId);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login attempt.";
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear(); //  optional, clean slate
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}

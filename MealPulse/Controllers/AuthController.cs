using Common;
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
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
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

        private bool IsPasswordValid(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => "!@#$%^&*".Contains(ch));
        }

        [HttpPost]
        public IActionResult Register(string FirstName, string LastName, string email, string password,
            DateTime date_of_birth, decimal height_cm, decimal weight_kg, int gender_id, int activityLevel_id, int metric_id)
        {
            ViewBag.GenderOptions = _authService.GetSelectListData("Gender", "gender_id", "gender");
            ViewBag.ActivityLevelOptions = _authService.GetSelectListData("ActivityLevel", "activityLevel_id", "activityLevel");
            ViewBag.MetricOptions = _authService.GetSelectListData("Metric", "metric_id", "metric");

            if (date_of_birth < new DateTime(1753, 1, 1) || date_of_birth > DateTime.Now)
            {
                ViewBag.ErrorMessage = "Please select a valid date of birth.";
                return View();
            }

            if (!IsPasswordValid(password))
            {
                ViewBag.ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, a number, and a special character (!@#$%^&*).";
                return View();
            }

            int age = DateTime.Today.Year - date_of_birth.Year;
            if (date_of_birth > DateTime.Today.AddYears(-age)) age--;

            if (age < ValidationConstraints.User.AgeMin + 12) // min 12 years old
            {
                ViewBag.ErrorMessage = "You must be at least 12 years old.";
                return View();
            }

            if (height_cm < (decimal)ValidationConstraints.User.HeightMin || height_cm > (decimal)ValidationConstraints.User.HeightMax)
            {
                ViewBag.ErrorMessage = $"Height must be between {ValidationConstraints.User.HeightMin}cm and {ValidationConstraints.User.HeightMax}cm.";
                return View();
            }


            if (_authService.UserExists(email))
            {
                ViewBag.ErrorMessage = "Email already registered. Please login.";
                return RedirectToAction("Login");
            }

            var token = Guid.NewGuid().ToString();
            var parameters = new Dictionary<string, object>
            {
                { "@FirstName", FirstName },
                { "@LastName", LastName },
                { "@email", email },
                { "@password", _authService.HashPassword(password) },
                { "@date_of_birth", date_of_birth },
                { "@height_cm", height_cm },
                { "@weight_kg", weight_kg },
                { "@role_id", 1 },
                { "@gender_id", gender_id },
                { "@activityLevel_id", activityLevel_id },
                { "@metric_id", metric_id },
                { "@token", token }
            };

            int userId = _authService.RegisterUser(parameters);
            if (userId > 0)
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                string confirmLink = $"{baseUrl}/Auth/ConfirmEmail?token={token}";
                _emailService.SendConfirmationEmail(email, confirmLink);

                TempData["SuccessMessage"] = "Registration successful. Please check your email to confirm your account.";
                return RedirectToAction("Login");
            }

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

                HttpContext.Session.SetInt32("user_id", userId);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login attempt or unconfirmed email.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ConfirmEmail(string token)
        {
            bool confirmed = _authService.ConfirmUserEmail(token);
            ViewBag.Confirmed = confirmed;
            return View("ConfirmEmailResult");
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            if (_authService.SendResetEmail(email))
            {
                TempData["Success"] = "Check your email for reset instructions.";
            }
            else
            {
                TempData["Error"] = "No user found with that email.";
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Invalid or expired reset link.";
                return RedirectToAction("ForgotPassword");
            }

            ViewBag.Token = token;
            return View();
        }



        [HttpPost]
        public IActionResult ResetPassword(string token, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                ViewBag.Token = token;
                return View();
            }

            if (!IsPasswordValid(password))
            {
                ViewBag.Error = "Password must be at least 8 characters and include uppercase, lowercase, a number, and a special character (!@#$%^&*).";
                ViewBag.Token = token;
                return View();
            }

            if (_authService.ResetPassword(token, password))
            {
                TempData["Success"] = "Password reset successfully.";
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Invalid or expired token.";
            ViewBag.Token = token;
            return View();
        }

    }
}

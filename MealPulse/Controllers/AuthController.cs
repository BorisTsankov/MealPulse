using Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using System.Security.Claims;
using Web.ViewModels;

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

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.GenderOptions = _authService.GetSelectListData("Gender", "gender_id", "gender");
            ViewBag.ActivityLevelOptions = _authService.GetSelectListData("ActivityLevel", "activityLevel_id", "activityLevel");
            ViewBag.MetricOptions = _authService.GetSelectListData("Metric", "metric_id", "metric");

            return View(new RegisterViewModel());
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
        public IActionResult Register(RegisterViewModel model)
        {
            ViewBag.GenderOptions = _authService.GetSelectListData("Gender", "gender_id", "gender");
            ViewBag.ActivityLevelOptions = _authService.GetSelectListData("ActivityLevel", "activityLevel_id", "activityLevel");
            ViewBag.MetricOptions = _authService.GetSelectListData("Metric", "metric_id", "metric");

            if (!IsPasswordValid(model.Password))
            {
                ModelState.AddModelError(nameof(model.Password), "Password must include uppercase, lowercase, number, and special character.");
            }

            if (model.DateOfBirth < new DateTime(1753, 1, 1) || model.DateOfBirth > DateTime.Now)
            {
                ModelState.AddModelError(nameof(model.DateOfBirth), "Please select a valid date of birth.");
            }

            var age = DateTime.Today.Year - model.DateOfBirth?.Year;
            if (model.DateOfBirth > DateTime.Today.AddYears(-age ?? 0)) age--;

            if (age < ValidationConstraints.User.AgeMin + 12)
            {
                ModelState.AddModelError(nameof(model.DateOfBirth), "You must be at least 12 years old.");
            }

            if (model.HeightCm < (decimal)ValidationConstraints.User.HeightMin || model.HeightCm > (decimal)ValidationConstraints.User.HeightMax)
            {
                ModelState.AddModelError(nameof(model.HeightCm), $"Height must be between {ValidationConstraints.User.HeightMin}cm and {ValidationConstraints.User.HeightMax}cm.");
            }

            if (_authService.UserExists(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), "Email is already registered. Please log in.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

\           var token = Guid.NewGuid().ToString();
            var parameters = new Dictionary<string, object>
    {
        { "@FirstName", model.FirstName },
        { "@LastName", model.LastName },
        { "@email", model.Email },
        { "@password", _authService.HashPassword(model.Password) },
        { "@date_of_birth", model.DateOfBirth },
        { "@height_cm", model.HeightCm },
        { "@weight_kg", model.WeightKg },
        { "@role_id", 1 },
        { "@gender_id", model.GenderId },
        { "@activityLevel_id", model.ActivityLevelId },
        { "@metric_id", model.MetricId },
        { "@token", token }
    };

            int userId = _authService.RegisterUser(parameters);
            if (userId > 0)
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                string confirmLink = $"{baseUrl}/Auth/ConfirmEmail?token={token}";
                _emailService.SendConfirmationEmail(model.Email, confirmLink);

                TempData["SuccessMessage"] = "Registration successful. Please check your email to confirm your account.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
            return View(model);
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

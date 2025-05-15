using Common;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Models;
using Services.Services.Interfaces;
using System.Data;
using System.Security.Cryptography;
using System.Text;


namespace Services.Services
{

    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGoalRepository _goalRepository;

        public AuthService(
            IAuthRepository authRepository,
            IHttpContextAccessor httpContextAccessor,
            IGoalRepository goalRepository)
        {
            _authRepository = authRepository;
            _httpContextAccessor = httpContextAccessor;
            _goalRepository = goalRepository;
        }


        public bool UserExists(string email)
        {
            return _authRepository.UserExists(email);
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public int RegisterUser(Dictionary<string, object> parameters)
        {
            int userId = _authRepository.RegisterUser(parameters);

            if (userId > 0)
            {
                decimal currentWeight = Convert.ToDecimal(parameters["@weight_kg"]);

                var initialGoal = new Goal
                {
                    user_id = userId,
                    current_weight_kg = currentWeight,
                    target_weight_kg = currentWeight, 
                    goal_intensity = (int)GoalIntensity.Maintain
                };

                _goalRepository.CreateGoal(initialGoal);
            }

            return userId;
        }



        public DataTable AuthenticateUser(string email, string password)
        {
            string hashed = HashPassword(password);
            return _authRepository.AuthenticateUser(email, hashed);
        }

        public List<SelectListItem> GetSelectListData(string tableName, string valueField, string textField)
        {
            DataTable dt = _authRepository.GetSelectListData(tableName, valueField, textField);
            return dt.AsEnumerable()
                     .Select(row => new SelectListItem
                     {
                         Value = row[valueField].ToString(),
                         Text = row[textField].ToString()
                     })
                     .ToList();
        }
        public string GetCurrentUserId()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                return userId ?? string.Empty;
            }

            return string.Empty;
        }
    }
}

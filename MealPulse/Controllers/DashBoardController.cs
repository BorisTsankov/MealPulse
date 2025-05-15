using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGoalService _goalService;
        private readonly IFoodDiaryService _foodDiaryService;

        public DashBoardController(IUserService userService, IGoalService goalService, IFoodDiaryService foodDiaryService)
        {
            _userService = userService;
            _goalService = goalService;
            _foodDiaryService = foodDiaryService;
        }

        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("Index", "Home");

            var user = _userService.GetUserById(userId);
            var goal = _goalService.GetMostRecentGoalByUserId(userId);
            if (user == null || goal == null)
                return View("Error");

            var items = _foodDiaryService.GetItemsByUserAndDate(userId, DateTime.Today);

            var totalCalories = items
                .Where(i => i.FoodItem != null)
                .Sum(i => (i.FoodItem.Calories * (decimal)i.Quantity) / 100);

            var calorieGoal = _goalService.CalculateCalorieGoal(user, goal);
            var remainingCalories = (calorieGoal ?? 0) - totalCalories;

            ViewBag.ConsumedCalories = (int)Math.Round(totalCalories);
            ViewBag.CalorieGoal = calorieGoal;
            ViewBag.RemainingCalories = remainingCalories > 0 ? (int)Math.Round(remainingCalories) : 0;

            return View();
        }
    }
}

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

        public DashBoardController(
            IUserService userService,
            IGoalService goalService,
            IFoodDiaryService foodDiaryService)
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

            decimal totalCalories = 0;
            decimal protein = 0, carbs = 0, fat = 0;
            decimal sugars = 0, fiber = 0, sodium = 0, potassium = 0, iron = 0, calcium = 0;

            foreach (var item in items.Where(i => i.FoodItem != null))
            {
                var factor = (decimal)item.Quantity / 100;
                var food = item.FoodItem;

                totalCalories += food.Calories * factor;
                protein += food.Protein * factor;
                carbs += food.Carbohydrates * factor;
                fat += food.Fat * factor;

                sugars += food.Sugars * factor;
                fiber += food.Fiber * factor;
                sodium += (decimal)(food.Sodium * factor);
                potassium += (decimal)(food.Potassium * factor);
                iron += (decimal)(food.Iron * factor);
                calcium += (decimal)(food.Calcium * factor);
            }

            var calorieGoal = _goalService.CalculateCalorieGoal(user, goal);
            var remainingCalories = (calorieGoal ?? 0) - totalCalories;

            // Macronutrient goals
            decimal goalProtein = 0, goalCarbs = 0, goalFat = 0;
            if (calorieGoal != null)
            {
                goalCarbs = calorieGoal.Value * 0.50m / 4;
                goalProtein = calorieGoal.Value * 0.20m / 4;
                goalFat = calorieGoal.Value * 0.30m / 9;
            }

            ViewBag.ConsumedCalories = (int)Math.Round(totalCalories);
            ViewBag.CalorieGoal = calorieGoal;
            ViewBag.RemainingCalories = remainingCalories > 0 ? (int)Math.Round(remainingCalories) : 0;

            ViewBag.ConsumedProtein = Math.Round(protein, 1);
            ViewBag.ConsumedCarbs = Math.Round(carbs, 1);
            ViewBag.ConsumedFat = Math.Round(fat, 1);
            ViewBag.GoalProtein = Math.Round(goalProtein, 1);
            ViewBag.GoalCarbs = Math.Round(goalCarbs, 1);
            ViewBag.GoalFat = Math.Round(goalFat, 1);

            // ✅ Add micronutrient values to ViewBag
            ViewBag.Sugars = Math.Round(sugars, 1);
            ViewBag.Fiber = Math.Round(fiber, 1);
            ViewBag.Sodium = Math.Round(sodium, 1);
            ViewBag.Potassium = Math.Round(potassium, 1);
            ViewBag.Iron = Math.Round(iron, 1);
            ViewBag.Calcium = Math.Round(calcium, 1);

            return View();
        }
    }
}

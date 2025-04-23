using Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using Core.Models;
using MealPulse.Services;
using MealPulse.Services.Interfaces;
using MealPulse.Models.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static MealPulse.Common.ValidationConstraints;

namespace Web.Controllers
{
    public class FoodDiaryController : Controller
    {
        private readonly IGoalService _goalService;
        private readonly IFoodDiaryService _foodDiaryService;
        private readonly IMealTypeService _mealTypeService;
        private readonly IFoodItemService _foodItemService;
        private readonly IUserService _userService;

        public FoodDiaryController(
            IGoalService goalService,
            IFoodDiaryService foodDiaryService,
            IMealTypeService mealTypeService,
            IFoodItemService foodItemService,
            IUserService userService)
        {
            _goalService = goalService;
            _foodDiaryService = foodDiaryService;
            _mealTypeService = mealTypeService;
            _foodItemService = foodItemService;
            _userService = userService;
        }

        public IActionResult Index(DateTime? date)
        {
            var userId = HttpContext.Session.GetInt32("user_id");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var goal = _goalService.GetMostRecentGoalByUserId(userId.Value);
            if (goal == null)
                return View("Error");

            var user = _userService.GetUserById(userId.Value);
            DateTime selectedDate = date?.Date ?? DateTime.Today;

            var foodItems = _foodDiaryService.GetItemsForGoalAndDate(goal.goal_id, selectedDate);
            var mealTypes = _mealTypeService.GetAll();

            var sections = mealTypes.Select(m => new FoodDiarySectionViewModel
            {
                MealName = m.MealTypeName,
                MealTypeId = m.MealTypeId,
                Items = foodItems
                    .Where(f => f.MealTypeId == m.MealTypeId)
                    .Select(f => new FoodDiaryItemViewModel
                    {
                        FoodName = f.FoodItem?.Name ?? "[Unknown]",
                        Quantity = (decimal)f.Quantity,
                        Unit = f.FoodItem?.Unit ?? "-",
                        Calories = f.FoodItem != null
                            ? (f.FoodItem.Calories * (decimal)f.Quantity) / 100
                            : 0
                    }).ToList(),

                TotalCalories = foodItems
                    .Where(f => f.MealTypeId == m.MealTypeId && f.FoodItem != null)
                    .Sum(f => (f.FoodItem.Calories * (decimal)f.Quantity) / 100)

            }).ToList();

            var totalCaloriesForDay = sections.Sum(s => s.TotalCalories);
            var calorieGoal = _goalService.CalculateCalorieGoal(user, goal); // Your new shared method

            ViewBag.FoodItems = _foodItemService.GetAll();
            ViewBag.TotalCaloriesForDay = totalCaloriesForDay;
            ViewBag.CalorieGoal = calorieGoal;
            ViewBag.RemainingCalories = calorieGoal != null ? calorieGoal - totalCaloriesForDay : null;
            ViewBag.SelectedDate = selectedDate;

            return View(new FoodDiaryViewModel
            {
                Sections = sections
            });
        }

        [HttpPost]
        public IActionResult AddItem(int MealTypeId, int FoodItemId, decimal Quantity, DateTime date)
        {
            var userId = HttpContext.Session.GetInt32("user_id");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var goal = _goalService.GetMostRecentGoalByUserId(userId.Value);
            if (goal == null)
                return View("Error");

            var success = _foodDiaryService.AddFoodDiaryItem(new FoodDiaryItem
            {
                GoalId = goal.goal_id,
                FoodId = FoodItemId,
                MealTypeId = MealTypeId,
                Quantity = (double)Quantity,
                DateTime = DateTime.Now
            });

            if (!success)
            {
                TempData["Error"] = "Something went wrong while adding the food item.";
            }

            return RedirectToAction("Index", new { date = date.ToString("yyyy-MM-dd") });
        }


    }
}

using DTOs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using System.Security.Claims;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize]
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("Index", "Home"); // sends them to landing page instead

            // use userId from here on


            var goal = _goalService.GetMostRecentGoalByUserId(userId);
            if (goal == null)
                return View("Error");

            var user = _userService.GetUserById(userId);
            DateTime selectedDate = date?.Date ?? DateTime.Today;

            var foodItems = _foodDiaryService.GetItemsByUserAndDate(userId, selectedDate);

            var mealTypes = _mealTypeService.GetAll();

            var sections = mealTypes.Select(m => new FoodDiarySectionViewModel
            {
                MealName = m.MealTypeName,
                MealTypeId = m.MealTypeId,
                Items = foodItems
                    .Where(f => f.MealTypeId == m.MealTypeId)
                    .Select(f => new FoodDiaryItemViewModel
                    {
                        FoodDiaryItemId = f.FoodDiaryItemId,
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

            //ViewBag.FoodItems = _foodItemService.GetAll();
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                Console.WriteLine("❌ user_id claim is missing or invalid.");
                return RedirectToAction("Login", "Auth");
            }

            var goal = _goalService.GetMostRecentGoalByUserId(userId);
            if (goal == null)
                return View("Error");

            var success = _foodDiaryService.AddFoodDiaryItem(new FoodDiaryItemDto
            {
                GoalId = goal.GoalId,
                FoodId = FoodItemId,
                MealTypeId = MealTypeId,
                Quantity = (double)Quantity,
                DateTime = date.Date
            });

            if (!success)
                TempData["Error"] = "Something went wrong while adding the food item.";

            return RedirectToAction("Index", new { date = date.ToString("yyyy-MM-dd") });
        }



        [HttpPost]
        public IActionResult DeleteItem(int id, DateTime date)
        {
            var success = _foodDiaryService.DeleteFoodDiaryItem(id);
            if (!success)
            {
                TempData["Error"] = "Something went wrong while deleting the food item.";
            }

            return RedirectToAction("Index", new { date = date.ToString("yyyy-MM-dd") });
        }

        [HttpGet]
        public IActionResult SearchFoodItems(string term)
        {
            var allItems = _foodItemService.SearchByName(term ?? "");

            var distinctItems = allItems
                .GroupBy(f => f.Name.Trim().ToLower())
                .Select(g => g.First()) // pick the first unique name
                .Take(20) // optional: limit results
                .ToList();

            var results = distinctItems.Select(f => new
            {
                id = f.FoodItemId,
                text = f.Name
            });

            return Json(new { results });
        }



    }
}

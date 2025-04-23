using Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces; // <-- Make sure this is the correct namespace
using Core.Models;
using MealPulse.Services;
using MealPulse.Services.Interfaces;
using MealPulse.Models.Models;

namespace Web.Controllers
{
    public class FoodDiaryController : Controller
    {
        private readonly IGoalService _goalService;
        private readonly IFoodDiaryService _foodDiaryService;
        private readonly IMealTypeService _mealTypeService;
        private readonly IFoodItemService _foodItemService;


        public FoodDiaryController(
            IGoalService goalService,
            IFoodDiaryService foodDiaryService,
            IMealTypeService mealTypeService,
              IFoodItemService foodItemService)
        {
            _goalService = goalService;
            _foodDiaryService = foodDiaryService;
            _mealTypeService = mealTypeService;
             _foodItemService = foodItemService;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("user_id");

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var goal = _goalService.GetMostRecentGoalByUserId(userId.Value);
            if (goal == null)
                return View("Error");

            var foodItems = _foodDiaryService.GetItemsForGoal(goal.goal_id);
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

            ViewBag.FoodItems = _foodItemService.GetAll();

            var viewModel = new FoodDiaryViewModel
            {
                Sections = sections
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult AddItem(int MealTypeId, int FoodItemId, decimal Quantity)
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

            return RedirectToAction("Index");
        }


    }
}

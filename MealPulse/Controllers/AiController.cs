using DTOs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Helpers;
using Services.Services;
using Services.Services.Interfaces;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
    public class AiController : Controller
    {
        private readonly AiService _aiService;
        private readonly IFoodDiaryService _foodDiaryService;
        private readonly IGoalService _goalService;
        private readonly IFoodItemService _foodItemService;

        public AiController(
            AiService aiService,
            IFoodDiaryService foodDiaryService,
            IGoalService goalService,
            IFoodItemService foodItemService)
        {
            _aiService = aiService;
            _foodDiaryService = foodDiaryService;
            _goalService = goalService;
            _foodItemService = foodItemService;
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] AiRequestDto input)
        {
            if (string.IsNullOrWhiteSpace(input?.Question))

                return Json(new { answer = "⚠️ Please ask a non-empty question." });

            var response = await _aiService.AskAsync(input.Question);

            if (response.TrimStart().StartsWith("{"))
            {
                try
                {
                    var parsed = JsonConvert.DeserializeObject<ChatFoodLog>(response);

                    Console.WriteLine(JsonConvert.SerializeObject(parsed));


                    int userId = GetCurrentUserId();
                    var goal = _goalService.GetMostRecentGoalByUserId(userId);
                    if (goal == null)
                        return Json(new { answer = "⚠️ You don't have a goal yet. Please set one in your profile." });

                    // Check if the food exists
                    var normalizedName = FoodNameHelper.Normalize(parsed.foodName);
                    var existing = _foodItemService.GetByName(normalizedName);

                    int foodItemId;

                    if (existing != null)
                    {
                        foodItemId = existing.FoodItemId ?? 0;
                    }
                    else
                    {
                        decimal factor = (decimal)parsed.quantity / 100m;
                        if (factor == 0) factor = 1;

                        var newItem = new FoodItemDto
                        {
                            Name = normalizedName,
                            Unit = parsed.unit,

                            // Normalize to 100g/ml
                            Calories = parsed.calories / factor,
                            Protein = parsed.protein / factor,
                            Fat = parsed.fat / factor,
                            Carbohydrates = parsed.carbohydrates / factor,
                            Sugars = parsed.sugars / factor,
                            Fiber = parsed.fiber / factor,
                            Sodium = parsed.sodium / factor,
                            Potassium = parsed.potassium / factor,
                            Iron = parsed.iron / factor,
                            Calcium = parsed.calcium / factor
                        };



                        foodItemId = _foodItemService.Add(newItem); // Make sure this returns the ID
                    }

                    var diaryItem = new FoodDiaryItemDto
                    {
                        GoalId = goal.GoalId,
                        FoodId = foodItemId,
                        MealTypeId = FoodLoggingHelper.GetMealTypeId(parsed.mealType),
                        Quantity = parsed.quantity,
                        DateTime = DateTime.UtcNow,
                        FoodName = parsed.foodName
                    };

                    bool success = _foodDiaryService.AddFoodDiaryItem(diaryItem);
                    return Json(new { answer = success ? "✅ Food logged successfully!" : "❌ Failed to log food." });
                }
                catch (Exception ex)
                {
                    return Json(new { answer = $"⚠️ Couldn't process the food log. {ex.Message}" });
                }
            }

            // Non-food messages (like questions)
            return Json(new { answer = response });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
                return userId;

            throw new Exception("User is not logged in.");
        }

        [HttpGet]
        public IActionResult AskView()
        {
            return View();
        }

        public class AiRequestDto
        {
            public string Question { get; set; }
        }
    }
}

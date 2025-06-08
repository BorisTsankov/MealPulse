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

            var historyJson = HttpContext.Session.GetString("ChatHistory");
            var messages = string.IsNullOrEmpty(historyJson)
                ? new List<ChatMessage>()
                : JsonConvert.DeserializeObject<List<ChatMessage>>(historyJson);

            messages.Add(new ChatMessage { Role = "user", Content = input.Question });

            var chatHistory = messages.Select(m => (m.Role, m.Content)).ToList();

            var response = await _aiService.AskAsync(chatHistory);

            if (response.TrimStart().StartsWith("{"))
            {
                try
                {
                    var parsed = JsonConvert.DeserializeObject<ChatFoodLog>(response);

                    int userId = GetCurrentUserId();
                    var goal = _goalService.GetMostRecentGoalByUserId(userId);
                    if (goal == null)
                    {
                        string msg = "⚠️ You don't have a goal yet. Please set one in your profile.";
                        messages.Add(new ChatMessage { Role = "ai", Content = msg });
                        HttpContext.Session.SetString("ChatHistory", JsonConvert.SerializeObject(messages));
                        return Json(new { answer = msg });
                    }

                    var normalizedName = FoodNameHelper.Normalize(parsed.foodName);
                    var existing = _foodItemService.GetByName(normalizedName);

                    int foodItemId = existing?.FoodItemId ?? 0;
                    if (existing == null)
                    {
                        decimal factor = (decimal)parsed.quantity / 100m;
                        if (factor == 0) factor = 1;

                        var newItem = new FoodItemDto
                        {
                            Name = normalizedName,
                            Unit = parsed.unit,
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

                        foodItemId = _foodItemService.Add(newItem);
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
                    var resultMsg = success ? "✅ Food logged successfully!" : "❌ Failed to log food.";

                    messages.Add(new ChatMessage { Role = "assistant", Content = resultMsg });

                    HttpContext.Session.SetString("ChatHistory", JsonConvert.SerializeObject(messages));
                    return Json(new { answer = resultMsg });
                }
                catch (Exception ex)
                {
                    string err = $"⚠️ Couldn't process the food log. {ex.Message}";
                    messages.Add(new ChatMessage { Role = "ai", Content = err });
                    HttpContext.Session.SetString("ChatHistory", JsonConvert.SerializeObject(messages));
                    return Json(new { answer = err });
                }
            }

            messages.Add(new ChatMessage { Role = "assistant", Content = response });
            HttpContext.Session.SetString("ChatHistory", JsonConvert.SerializeObject(messages));
            return Json(new { answer = response });
        }

        [HttpGet]
        public IActionResult AskView()
        {
            var history = HttpContext.Session.GetString("ChatHistory");
            var messages = string.IsNullOrEmpty(history)
                ? new List<ChatMessage>()
                : JsonConvert.DeserializeObject<List<ChatMessage>>(history);

            ViewBag.ChatHistoryJson = JsonConvert.SerializeObject(messages);
            return View();
        }

        [HttpPost]
        public IActionResult ClearChat()
        {
            HttpContext.Session.Remove("ChatHistory");
            return Ok();
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
                return userId;

            throw new Exception("User is not logged in.");
        }

        public class AiRequestDto
        {
            public string Question { get; set; }
        }

        public class ChatMessage
        {
            public string Role { get; set; }
            public string Content { get; set; }
        }
    }
}

using MealPulse.ViewModels;
using MealPulse.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using System.Security.Claims;
using MealPulse.Models.Models;
using Core.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MealPulse.Common.ValidationConstraints;


[Authorize]
public class ProfileController : Controller
{
    private readonly IUserService _userService;
    private readonly IGoalService _goalService;
    private readonly IGenderService _genderService;
    private readonly IActivityLevelService _activityLevelService;
    private readonly IMetricService _metricService;

    public ProfileController(
        IUserService userService,
        IGoalService goalService,
        IGenderService genderService,
        IActivityLevelService activityLevelService,
        IMetricService metricService)
    {
        _userService = userService;
        _goalService = goalService;
        _genderService = genderService;
        _activityLevelService = activityLevelService;
        _metricService = metricService;
    }

    public IActionResult Index()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return View("Error");

        var user = _userService.GetUserById(userId);
        if (user == null) return View("Error");

        var goal = _goalService.GetMostRecentGoalByUserId(user.user_id);

        // Provide intensity options to the view
        ViewBag.IntensityOptions = Enum.GetValues(typeof(GoalIntensity))
            .Cast<GoalIntensity>()
            .Select(g => new SelectListItem
            {
                Value = ((int)g).ToString(),
                Text = SplitCamelCase(g.ToString())
            }).ToList();
        ViewBag.ActivityLevelOptions = _activityLevelService.GetAll()
    .Select(a => new SelectListItem
    {
        Value = a.ActivityLevelId.ToString(),
        Text = a.ActivityLevelName
    }).ToList();

        // Basic input
        var height = user.height_cm;
        var weight = goal?.current_weight_kg;
        var birthdate = user.date_of_birth;
        var gender = _genderService.GetGenderName(user.gender_id);
        var activityLevel = _activityLevelService.GetActivityLevelName(user.activityLevel_id);
        var metric = _metricService.GetMetricNameById(user.metric_id);
        var goalIntensity = goal != null ? (GoalIntensity)goal.goal_intensity : GoalIntensity.Maintain;

        // Skip if anything is missing
        int? dailyCalories = null;
        if (height != null && weight != null && birthdate != null && gender != null && activityLevel != null)
        {
            int age = DateTime.Today.Year - birthdate.Value.Year;
            if (birthdate > DateTime.Today.AddYears(-age)) age--;

            // BMR (Mifflin-St Jeor)
            double bmr = (10 * (double)weight) + (6.25 * (double)height) - (5 * age) + (gender?.Trim().ToLower() == "male" ? 5 : -161);


            // Activity level multiplier
            var multiplier = activityLevel.ToLower() switch
            {
                "not active" => 1.2,
                "slightly active" => 1.375,
                "average" => 1.55,
                "above average" => 1.725,
                "very active" => 1.9,
                _ => 1.2
            };

            double tdee = bmr * multiplier;

            // Goal adjustment
            var adjustment = goalIntensity switch
            {
                GoalIntensity.LoseQuarterKgPerWeek => -250,
                GoalIntensity.LoseHalfKgPerWeek => -500,
                GoalIntensity.LoseOneKgPerWeek => -1000,
                GoalIntensity.GainQuarterKgPerWeek => 250,
                GoalIntensity.GainHalfKgPerWeek => 500,
                GoalIntensity.GainOneKgPerWeek => 1000,
                _ => 0
            };

            dailyCalories = (int)Math.Round(tdee + adjustment);
        }

       
        var viewModel = new UserProfileViewModel
        {
            User = user,
            Goal = goal,
            GoalIntensityDisplay = goal != null ? SplitCamelCase(((GoalIntensity)goal.goal_intensity).ToString()) : "",
            GenderName = gender?.ToString()?.Trim() ?? "Not set",
            ActivityLevelName = activityLevel?.ToString()?.Trim() ?? "Not set",
            MetricName = metric?.ToString()?.Trim() ?? "Not set",
            DailyCalories = dailyCalories
        };

      



        return View(viewModel);
    }

    public string SplitCamelCase(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, "(\\B[A-Z])", " $1");
    }

    [HttpPost]
    public IActionResult UpdateHeight(int user_id, decimal newHeight)
    {
        _userService.UpdateHeight(user_id, newHeight);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult UpdateWeight(int user_id, decimal newWeight)
    {
        _goalService.UpdateWeight(user_id, newWeight);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult CreateGoal(int user_id, decimal currentWeight, decimal targetWeight, string intensity)
    {
        var user = _userService.GetUserById(user_id);
        if (user == null)
        {
            return View("Error");
        }

        var newGoal = new MealPulse.Models.Models.Goal
        {
            user_id = user_id,
            current_weight_kg = currentWeight,
            target_weight_kg = targetWeight,
            start_date = DateTime.Today,
            goal_intensity = (int)Enum.Parse<GoalIntensity>(intensity)
        };

        var success = _goalService.CreateGoal(newGoal);
        if (!success)
        {
            return View("Error");
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult UpdateActivityLevel(int user_id, int newActivityLevelId)
    {
        _userService.UpdateActivityLevel(user_id, newActivityLevelId);
        return RedirectToAction("Index");
    }



}

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
// other usings...

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

        var gender = _genderService.GetGenderName(user.gender_id);
        var activityLevel = _activityLevelService.GetActivityLevelName(user.activityLevel_id);
        var metric = _metricService.GetMetricNameById(user.metric_id);






        var viewModel = new UserProfileViewModel
        {
            User = user,
            Goal = goal,
            GoalIntensityDisplay = goal != null ? SplitCamelCase(((GoalIntensity)goal.goal_intensity).ToString()) : "",
            GenderName = gender?.ToString()?.Trim() ?? "Not set",
            ActivityLevelName = activityLevel?.ToString()?.Trim() ?? "Not set",
            MetricName = metric?.ToString()?.Trim() ?? "Not set",




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


}

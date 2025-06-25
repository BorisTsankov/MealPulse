using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Common;
using Services.Services.Interfaces;
using System.Security.Claims;
using Web.ViewModels;


namespace Web.Controllers
{

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

            var goal = _goalService.GetMostRecentGoalByUserId(user.UserId);

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

            int? dailyCalories = (goal != null)
                ? _goalService.CalculateCalorieGoal(user, goal)
                : null;

            var viewModel = new UserProfileViewModel
            {
                User = user,
                Goal = goal,
                GoalIntensityDisplay = goal != null
                    ? SplitCamelCase(((GoalIntensity)goal.GoalIntensity).ToString())
                    : "",
                DailyCalories = dailyCalories
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateHeight(int user_id, decimal newHeight)
        {
            if (newHeight < (decimal)ValidationConstraints.User.HeightMin || newHeight > (decimal)ValidationConstraints.User.HeightMax)
            {
                TempData["ErrorMessage"] = "Height must be between 120 and 300 cm.";
                return RedirectToAction("Index");
            }

            _userService.UpdateHeight(user_id, newHeight);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult UpdateWeight(int user_id, decimal newWeight)
        {
            if (!_goalService.UpdateWeight(user_id, newWeight))
            {
                TempData["ErrorMessage"] = "Weight must be between 40 and 300 kg.";
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult CreateGoal(int user_id, decimal currentWeight, decimal targetWeight, string intensity)
        {
            var user = _userService.GetUserById(user_id);
            if (user == null) return View("Error");

            var success = _goalService.CreateGoal(user_id, currentWeight, targetWeight, intensity);
            if (!success)
            {
                TempData["ErrorMessage"] = "Weights must be between 40 and 300 kg.";
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateActivityLevel(int user_id, int newActivityLevelId)
        {
            _userService.UpdateActivityLevel(user_id, newActivityLevelId);
            return RedirectToAction("Index");
        }

        public string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "(\\B[A-Z])", " $1");
        }
    }
}

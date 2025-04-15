using MealPulse.ViewModels;
using MealPulse.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using System.Security.Claims;
// other usings...

[Authorize]
public class ProfileController : Controller
{
    private readonly IUserService _userService;
    private readonly IGoalService _goalService;

    public ProfileController(IUserService userService, IGoalService goalService)
    {
        _userService = userService;
        _goalService = goalService;
    }


    public IActionResult Index()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return View("Error");

        var user = _userService.GetUserById(userId);
        if (user == null) return View("Error");

        var goal = _goalService.GetMostRecentGoalByUserId(user.user_id);

        var viewModel = new UserProfileViewModel
        {
            User = user,
            Goal = goal
        };

        return View(viewModel);
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

}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MealPulse.Services.Interfaces;

namespace MealPulse.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return View("Error");

            var user = _userService.GetUserById(userId);
            return user == null ? View("Error") : View(user);
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
            _userService.UpdateWeight(user_id, newWeight);
            return RedirectToAction("Index");
        }
    }
}

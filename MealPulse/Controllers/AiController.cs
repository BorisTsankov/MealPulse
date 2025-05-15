using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace Web.Controllers
{
    [Authorize]
    public class AiController : Controller
    {
        private readonly AiService _aiService;

        public AiController(AiService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] AiRequestDto input)
        {
            if (string.IsNullOrWhiteSpace(input?.Question))
            {
                return Json(new { answer = "⚠️ Please ask a non-empty question." });
            }

            var answer = await _aiService.AskAsync(input.Question);
            return Json(new { answer });
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

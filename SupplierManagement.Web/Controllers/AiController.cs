using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Web.Services;
using SupplierManagement.Web.Models.Revenue;
using SupplierManagement.Web.MVCDTO;

namespace SupplierManagement.Web.Controllers
{
    public class AiController : Controller
    {
        private readonly MVCAiService _aiService;

        public AiController(MVCAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpGet]
        public IActionResult RevenueInsights()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Chat()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AskQuestion([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Question))
                return BadRequest(new { error = "Question cannot be empty." });

            var data = await _aiService.AskAsync(request.Question);

            if (!string.IsNullOrEmpty(data.ErrorMessage))
                return StatusCode(500, new { error = data.ErrorMessage });

            return Ok(new { aiAnswer = data.AiAnswer });
        }

    }
}
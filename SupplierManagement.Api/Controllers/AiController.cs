using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Api.AI;
using SupplierManagement.Data.DTO;
using SupplierManagement.Business.Interfaces;
namespace SupplierManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;
        private readonly IRevenueSkill _revenueSkill;

        public AiController(IAiService aiService, IRevenueSkill revenueSkill)
        {
            _aiService = aiService;
            _revenueSkill = revenueSkill;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var result = await _aiService.GetCompletionAsync("Hello GPT");
            return Ok(new { response = result });
        }
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Question))
                return BadRequest(new { error = "Question cannot be empty." });

            var topSuppliers = await _revenueSkill.GetTopSuppliersAsync(5);

            if (!topSuppliers.Any())
                return Ok(new { topSuppliers, aiAnswer = "No revenue data available to answer your question." });

            var prompt = AiService.BuildRevenueInsightsPrompt(topSuppliers, request.Question);
            var aiAnswer = await _aiService.GetCompletionAsync(prompt);

            return Ok(new { topSuppliers, aiAnswer });
        }
    }
}
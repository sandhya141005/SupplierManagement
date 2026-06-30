using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Api.AI;
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
        [HttpGet("revenue-insights")]
        public async Task<IActionResult> RevenueInsights()
        {
            var topSuppliers = await _revenueSkill.GetTopSuppliersAsync(5);

            if (!topSuppliers.Any())
                return Ok(new { summary = "No revenue data available yet." });

            var prompt = AiService.BuildRevenueInsightsPrompt(topSuppliers);
            var aiResponse = await _aiService.GetCompletionAsync(prompt);

            return Ok(new
            {
                topSuppliers,
                aiSummary = aiResponse
            });
        }

    }
}
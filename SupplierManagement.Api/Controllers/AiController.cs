using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Api.AI;
using SupplierManagement.Data.DTO;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Business.Services;
namespace SupplierManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;
        private readonly IRevenueSkill _revenueSkill;
        private readonly IInventorySkill _inventorySkill;
        private readonly IOrderSkill _orderSkill;

        public AiController(IAiService aiService, IRevenueSkill revenueSkill, IInventorySkill inventorySkill, IOrderSkill orderSkill)
        {
            _aiService = aiService;
            _revenueSkill = revenueSkill;
            _inventorySkill = inventorySkill;
            _orderSkill = orderSkill;
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

            var skill = QuestionRouter.Route(request.Question);

            if (skill == SkillType.Unknown)
                return Ok(new { aiAnswer = "I currently support revenue and inventory analysis." });

            if (skill == SkillType.Revenue)
            {
                var suppliers = await _revenueSkill.GetSuppliersAsync();

                if (!suppliers.Any())
                    return Ok(new { aiAnswer = "No revenue data available to answer your question." });

                var prompt = AiService.BuildRevenueInsightsPrompt(suppliers, request.Question);
                var answer = await _aiService.GetCompletionAsync(prompt);
                return Ok(new { aiAnswer = answer });
            }

            if (skill == SkillType.Inventory)
            {
                var products = await _inventorySkill.GetStocksAsync();

                if (!products.Any())
                    return Ok(new { aiAnswer = "No inventory data available to answer your question." });

                var prompt = AiService.BuildInventoryInsightsPrompt(products, request.Question);
                var answer = await _aiService.GetCompletionAsync(prompt);
                return Ok(new { aiAnswer = answer });
            }
            if (skill == SkillType.Order)
            {
                var orders = await _orderSkill.GetOrderSummaryAsync();

                if (!orders.Any())
                    return Ok(new { aiAnswer = "No order data available to answer your question." });

                var prompt = AiService.BuildOrderInsightsPrompt(orders, request.Question);
                var answer = await _aiService.GetCompletionAsync(prompt);
                return Ok(new { aiAnswer = answer });
            }
            return Ok(new { aiAnswer = "I currently support revenue, orders and inventory analysis." });

        }

    }
}
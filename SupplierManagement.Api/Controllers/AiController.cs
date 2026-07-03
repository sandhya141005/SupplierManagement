using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Api.AI;
using SupplierManagement.Data.DTO;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Business.Services;
using SupplierManagement.Business.Agent;
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
        private readonly IAgentService _agent;

        public AiController(IAiService aiService, IRevenueSkill revenueSkill, IInventorySkill inventorySkill, IOrderSkill orderSkill, IAgentService agent)
        {
            _aiService = aiService;
            _revenueSkill = revenueSkill;
            _inventorySkill = inventorySkill;
            _orderSkill = orderSkill;
            _agent = agent;
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

            var answer = await _agent.AskAsync(request.Question);
            return Ok(new { aiAnswer = answer });
        }
    }
}
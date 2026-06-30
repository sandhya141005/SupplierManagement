using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Api.AI;
namespace SupplierManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;

        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var result = await _aiService.GetCompletionAsync("Hello GPT");
            return Ok(new { response = result });
        }
    }
}
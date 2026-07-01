using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Web.Services;
using SupplierManagement.Web.Models.Revenue;

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
        public async Task<IActionResult> GetInsights(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return PartialView("_RevenueInsightsResult", new RevenueInsightsModel
                {
                    ErrorMessage = "Please enter a question."
                });

            var data = await _aiService.GetRevenueInsightsAsync(question);
            return PartialView("_RevenueInsightsResult", data);
        }
    }
}
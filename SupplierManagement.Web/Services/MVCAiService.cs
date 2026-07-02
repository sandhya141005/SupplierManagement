using System.Text.Json;
using SupplierManagement.Web.Models;
using SupplierManagement.Web.Models.Revenue;
namespace SupplierManagement.Web.Services
{
    public class MVCAiService
    {
        private readonly HttpClient _httpClient;
        public MVCAiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<RevenueInsightsModel> GetRevenueInsightsAsync(string question)
        {
            var model = new RevenueInsightsModel { Question = question };
            try
            {
                var payload = JsonSerializer.Serialize(new { question });
                var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/ai/chat", content);
                if (!response.IsSuccessStatusCode)
                {
                    model.ErrorMessage = "Failed to fetch insights";
                    return model;
                }
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("aiAnswer", out var answerEl))
                    model.AiAnswer = answerEl.GetString();

            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
            }

            return model;
        }
    }
}
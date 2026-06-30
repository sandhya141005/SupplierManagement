using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SupplierManagement.Business.DTO;
namespace SupplierManagement.Api.AI
{
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apikey;
        private readonly string _model;
        public AiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apikey = config["Groq:ApiKey"];
            _model = config["Groq:Model"] ?? "llama-3.3-70b-versatile";
        }
        public async Task<string> GetCompletionAsync(string userMessage)
        {
            var req = new GroqRequest
            {
                model = _model,
                messages = new List<GroqMessage>
                {
                    new GroqMessage{role="system",content="You are a helpful assistant with revenue,finance, math knowledge"},
                    new() {role="user",content=userMessage}

                },
                max_tokens = 500
            };
            var json = JsonSerializer.Serialize(req);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apikey);
            var response = await _httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(responseJson);
            var parsed = JsonSerializer.Deserialize<GroqResponse>(responseJson);
            return parsed?.choices?[0]?.message?.content ?? "No response.";

        }
        public static string BuildRevenueInsightsPrompt(List<SupplierRevenueDTO> suppliers)
        {
            var sb = new StringBuilder();
            sb.AppendLine("You are a financial business analyst reviewing supplier revenue data.");
            sb.AppendLine("Below is the Top 5 suppliers ranked by revenue, already calculated from our database.");
            sb.AppendLine("Do NOT recalculate any numbers. Only analyze the data given.");
            sb.AppendLine();
            sb.AppendLine("Top 5 Suppliers:");

            int rank = 1;
            foreach (var s in suppliers)
            {
                sb.AppendLine($"{rank}. {s.CompanyName} — Revenue: ₹{s.Revenue:N0}, Orders: {s.OrderCount}");
                rank++;
            }

            sb.AppendLine();
            sb.AppendLine("Based on this data, provide:");
            sb.AppendLine("1. Executive Summary — identify the best-performing supplier.");
            sb.AppendLine("2. Revenue Trends — state whether revenue is concentrated among a few suppliers or evenly spread.");
            sb.AppendLine("3. Supplier Performance Analysis — highlight weaker suppliers in this list.");
            sb.AppendLine("4. Business Risks — any risk from over-reliance on top suppliers.");
            sb.AppendLine("5. Recommendations — give 2-3 concise, actionable financial recommendations.");
            sb.AppendLine();
            sb.AppendLine("Keep the entire response under 200 words. Be direct and business-focused, no fluff.");

            return sb.ToString();
        }
    }
}
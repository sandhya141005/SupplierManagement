using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SupplierManagement.Data.DTO;
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
            //Console.WriteLine(response.StatusCode);
            //Console.WriteLine(responseJson);
            var parsed = JsonSerializer.Deserialize<GroqResponse>(responseJson);
            return parsed?.choices?[0]?.message?.content ?? "No response.";

        }
        public static string BuildRevenueInsightsPrompt(List<SupplierRevenueDTO> suppliers, string userQuestion)
        {
            var sb = new StringBuilder();
            sb.AppendLine("You are a financial business analyst for a supplier management platform.");
            sb.AppendLine("You have been given pre-calculated supplier revenue data from the database.");
            sb.AppendLine("Answer the user's question using ONLY the data provided below.");
            sb.AppendLine("Do NOT make up numbers. Do NOT assume data not given.");
            sb.AppendLine();
            sb.AppendLine(" BUSINESS DATA ");
            foreach (var s in suppliers)
            {
                sb.AppendLine($" {s.CompanyName} — Revenue: ₹{s.Revenue}, Orders: {s.OrderCount}");
        
            }
            sb.AppendLine();
            sb.AppendLine(" USER QUESTION ");
            sb.AppendLine(userQuestion);
            sb.AppendLine();
            sb.AppendLine("Answer concisely and professionally. Keep response under 150 words.");

            return sb.ToString();
        }


    }
}
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SupplierManagement.Data.DTO;
namespace SupplierManagement.Api.AI
{
    //cheking brancheeeeeeeeeeeee
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
            sb.AppendLine("You are a financial business analyst.");
            sb.AppendLine("You have been given the complete supplier revenue dataset from the database.");
            sb.AppendLine("Answer the user's question using ONLY the data provided below.");
            sb.AppendLine("You may perform comparisons, rankings, or aggregations on this data to answer the question.");
            sb.AppendLine("Do NOT make up data not present in this list.");
            sb.AppendLine();
            sb.AppendLine(" COMPLETE SUPPLIER REVENUE DATA ");
            foreach (var s in suppliers)
            {
                sb.AppendLine($" {s.CompanyName} — Revenue: ₹{s.Revenue:N0}, Orders: {s.OrderCount}");
            }

            sb.AppendLine($"Total suppliers: {suppliers.Count}");
            sb.AppendLine();
            sb.AppendLine(" USER QUESTION ");
            sb.AppendLine(userQuestion);
            sb.AppendLine();
            sb.AppendLine("Answer concisely and professionally. Keep the stuff youre wondering to yourself and tell the accurate data alone with the stats.Keep response under 150 words.");


            return sb.ToString();
        }
        public static string BuildInventoryInsightsPrompt(List<ProductInventoryDTO> products, string userQuestion)
        {
            var sb = new StringBuilder();
            sb.AppendLine("You are a supply chain analyst for a supplier management platform.");
            sb.AppendLine("You have been given pre-fetched inventory data from the database.");
            sb.AppendLine("Answer the user's question using ONLY the data provided below.");
            sb.AppendLine("Do NOT make up numbers. Do NOT assume data not given.");
            sb.AppendLine();
            sb.AppendLine("INVENTORY DATA ");
            foreach (var p in products)
            {
                sb.AppendLine($" {p.ProductName} (Supplier: {p.CompanyName}) — Stock: {p.StockQuantity} units");

            }
            sb.AppendLine($"Total products: {products.Count}");
            sb.AppendLine();
            sb.AppendLine("USER QUESTION ");
            sb.AppendLine(userQuestion);
            sb.AppendLine();
            sb.AppendLine("Answer concisely and professionally. Keep the stuff youre wondering to yourself and tell the accurate data alone with the stats.Keep response under 150 words.");

            return sb.ToString();
        }
        public static string BuildOrderInsightsPrompt(List<OrderSummaryDTO> orders, string userQuestion)
        {
            var sb = new StringBuilder();
            sb.AppendLine("You are a business operations analyst.");
            sb.AppendLine("You have been given the complete order summary dataset from the database.");
            sb.AppendLine("Answer the user's question using ONLY the data provided below.");
            sb.AppendLine("You may perform comparisons, rankings, or aggregations on this data to answer the question.");
            sb.AppendLine("Do NOT make up data not present in this list.");
            sb.AppendLine();
            sb.AppendLine(" COMPLETE ORDER SUMMARY DATA ");
            foreach (var o in orders)
            {
                sb.AppendLine(
                    $"Product: {o.ProductName}, " +
                    $"Category: {o.Category}, " +
                    $"Supplier: {o.CompanyName}, " +
                    $"Units Sold: {o.UnitsSold}, " +
                    $"Revenue: ₹{o.Revenue}, " +
                    $"Current Stock: {o.CurrentStock}, " +
                    $"Avg Daily Sales: {o.AvgDailySales:F2}, " +
                    $"Days Until Stockout: {o.EstimatedDaysUntilStockout:F1}"
                );
            }

            sb.AppendLine($"Total suppliers with orders: {orders.Count}");
            sb.AppendLine();
            sb.AppendLine("USER QUESTION");
            sb.AppendLine(userQuestion);
            sb.AppendLine();
            sb.AppendLine("Answer concisely and professionally. Keep the stuff youre wondering to yourself and tell the accurate data alone with the stats.Keep response under 150 words.");

            return sb.ToString();
        }
    }
}
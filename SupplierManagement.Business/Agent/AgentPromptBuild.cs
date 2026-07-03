using System.Text;

namespace SupplierManagement.Business.Agent
{
    public static class AgentPromptBuild
    {
        public static string Build(AgentContext ctx)
        {
            var sb = new StringBuilder();

            sb.AppendLine("You are an Operations Agent — a senior business analyst for a supplier management platform.");
            sb.AppendLine("You have been given pre-calculated business data from the database.");
            sb.AppendLine("Answer the user's question using ONLY the data provided.");
            sb.AppendLine("Do NOT invent numbers. Do NOT assume data not present.");
            sb.AppendLine("Structure your response as:");
            sb.AppendLine("  RECOMMENDATION: (one clear action or answer)");
            sb.AppendLine("  REASON: (2-4 bullet points explaining why)");
            sb.AppendLine("  CONFIDENCE: (High / Medium / Low — based on data completeness)");
            sb.AppendLine("Keep total response under 200 words.");
            sb.AppendLine();
            if (ctx.CalculatedInsights.Any())
            {
                sb.AppendLine("PRODUCT INSIGHTS (calculated)");
                foreach (var p in ctx.CalculatedInsights)
                {
                    sb.AppendLine($"- {p.ProductName} | Supplier: {p.SupplierName} | Category: {p.Category}");
                    sb.AppendLine($"  Stock: {p.AvailableStock} units | Sold: {p.TotalUnitsSold} units | Avg Daily Sales: {p.AvgDailySales}/day | Days to Stockout: {(p.DaysToStockout == 999 ? "N/A" : p.DaysToStockout)} | At Risk: {p.IsAtRisk} | Slow Moving: {p.IsSlowMoving}");
                }
                sb.AppendLine();
            }
            if (ctx.RevenueData.Any())
            {
                sb.AppendLine("SUPPLIER REVENUE DATA");
                foreach (var s in ctx.RevenueData)
                {
                    sb.AppendLine($" {s.CompanyName} — Revenue: ₹{s.Revenue:N0}, Orders: {s.OrderCount}");
                }
                sb.AppendLine();
            }
            if (ctx.OrderData.Any())
            {
                sb.AppendLine("ORDER SUMMARY DATA");
                foreach (var od in ctx.OrderData)
                {
                    sb.AppendLine(
                    $"Product: {od.ProductName}, " +
                    $"Category: {od.Category}, " +
                    $"Supplier: {od.CompanyName}, " +
                    $"Units Sold: {od.UnitsSold}, " +
                    $"Revenue: ₹{od.Revenue}, " +
                    $"Current Stock: {od.CurrentStock}, " +
                    $"Avg Daily Sales: {od.AvgDailySales:F2}, " +
                    $"Days Until Stockout: {od.EstimatedDaysUntilStockout:F1}"
                );
                }
                sb.AppendLine();
            }
            sb.AppendLine(" USER QUESTION ");
            sb.AppendLine(ctx.UserQuestion);

            return sb.ToString();
        }
    }
}
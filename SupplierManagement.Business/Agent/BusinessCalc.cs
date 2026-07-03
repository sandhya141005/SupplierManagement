using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data.DTO;
namespace SupplierManagement.Business.Agent
{
    public class BusinessCalc
    {
        private const int Baseline = 30;
        private const int StockoutRiskdays = 7;
        private const int SlowMoving = 5;
        public static List<ProductInsightsDTO> Calculate(List<ProductInventoryDTO> inventory, List<OrderSummaryDTO> orders)
        {
            var insights = new List<ProductInsightsDTO>();
            foreach (var prod in inventory)
            {
                int tot = prod.TotalUnitsSold;
                decimal avgDailySales = Math.Round((decimal)tot / Baseline, 2);
             /*   Console.WriteLine($"{prod.ProductName} " +$"Sold={prod.TotalUnitsSold} " +
               $"Avg={avgDailySales}");*/
                int daysToStockout = avgDailySales > 0 ? (int)Math.Floor(prod.StockQuantity / avgDailySales) : 999;
                insights.Add(new ProductInsightsDTO
                {
                    ProductName = prod.ProductName,
                    SupplierName = prod.CompanyName,
                    Category = prod.Category,
                    AvailableStock = prod.StockQuantity,
                    TotalUnitsSold = tot,
                    AvgDailySales = avgDailySales,
                    DaysToStockout = daysToStockout,
                    IsAtRisk = daysToStockout <= StockoutRiskdays,
                    IsSlowMoving = tot <= SlowMoving
                });
            }

            return insights
                .OrderBy(x => x.DaysToStockout)
                .ToList();
        }

    }

}

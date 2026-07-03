namespace SupplierManagement.Data.DTO
{   
public class ProductInsightsDTO
    {
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public string Category { get; set; }
        public int AvailableStock { get; set; }
        public int TotalUnitsSold { get; set; }
        public decimal AvgDailySales { get; set; }
        public int DaysToStockout { get; set; }
        public bool IsAtRisk { get; set; }
        public bool IsSlowMoving { get; set; }
    }
}
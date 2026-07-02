public class OrderSummaryDTO
    {
       public string ProductName { get; set; } = "";
    public string Category { get; set; } = "";
    public string CompanyName { get; set; } = "";
    public int UnitsSold { get; set; }
    public int TotalOrders { get; set; }
    public decimal Revenue { get; set; }
    public int CurrentStock { get; set; }
    public double AvgUnitsPerOrder { get; set; }
    public double AvgDailySales { get; set; }
    public double EstimatedDaysUntilStockout { get; set; }
    }
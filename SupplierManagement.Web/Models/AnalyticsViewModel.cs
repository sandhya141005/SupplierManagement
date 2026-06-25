namespace SupplierManagement.Web.Models;

public class SupplierRevenueViewModel
{
    public string SupplierName { get; set; } = "";
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
}

public class CountrySalesViewModel
{
    public string CountryName { get; set; } = "";
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
}

public class AnalyticsViewModel
{
    public List<SupplierRevenueViewModel> SupplierRevenue { get; set; } = new();
    public List<CountrySalesViewModel> CountrySales { get; set; } = new();
}
namespace SupplierManagement.Api.DTO;

public class SupplierRevenueDTO
{
    public string SupplierName { get; set; } = "";
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
}
public class CountrySalesDTO
{
    public string CountryName { get; set; } = "";
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
}
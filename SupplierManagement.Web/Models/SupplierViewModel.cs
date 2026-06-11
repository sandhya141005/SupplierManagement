namespace SupplierManagement.Web.Models;

public class SupplierViewModel
{
    public int SupplierId { get; set; }
    public string CompanyName { get; set; } = "";
    public int TotalProducts { get; set; }
    public string CatalogType { get; set; } = "";
    public string PaymentMethodsAllowed { get; set; } = "";
    public string CreatedDate { get; set; } = "";
    public string Country { get; set; } = "";
    public string State { get; set; } = "";
    public string City { get; set; } = "";
    public string ContactNo { get; set; } = "";
}
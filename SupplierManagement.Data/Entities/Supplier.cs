namespace SupplierManagement.Data.Entities;

public class Supplier
{
    public int SupplierId { get; set; }
    public string CompanyName { get; set; } = "";
    public int TotalProducts { get; set; }
    public string CatalogType { get; set; } = "";
    public string PaymentMethodsAllowed { get; set; } = "";
    public DateTime CreatedDate { get; set; }
    public int CountryId { get; set; }
    public int StateId { get; set; }
    public int CityId { get; set; }
    public string? ContactNo { get; set; } 
      public List<Product> Products { get; set; } = new();
}
namespace SupplierManagement.Api.DTO;

public class SupplierDTO
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
    public string? ContactNo { get; set; }
    public int CountryId { get; set; }
    public int StateId { get; set; }
    public int CityId { get; set; }
    public List<ProductDTO> Products { get; set; } = new();
    public List<int> DeletedProductIds { get; set; } = new();
}
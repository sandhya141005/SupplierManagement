using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SupplierManagement.Web.Models;

public class SupplierViewModel
{
    public int SupplierId { get; set; }

    [Required(ErrorMessage = "Company name is required")]
    public string CompanyName { get; set; } = "";
    [Required(ErrorMessage = "Total products is required")]
    public int TotalProducts { get; set; }
    [Required(ErrorMessage = "Catalog type is required")]
    public string CatalogType { get; set; } = "";
    [ValidateNever]
    public string PaymentMethodsAllowed { get; set; } = "";
    [ValidateNever]
    public List<string> SelectedPaymentMethods { get; set; } = new();
    [Required(ErrorMessage = "Created date is required")]
    [RegularExpression(@"^\d{2}-[A-Za-z]{3}-\d{4}$", ErrorMessage = "Date must be in dd-Mon-yyyy format e.g. 11-Jun-2026")]
    [JsonIgnore]
    public string CreatedDate { get; set; } = "";
    [ValidateNever]

    [JsonPropertyName("createdDate")]
    public DateTime? CreatedDateRaw { get; set; }
    public string Country { get; set; } = "";
    public string State { get; set; } = "";
    public string City { get; set; } = "";
   
    public string? ContactNo { get; set; }

    [Required(ErrorMessage = "Please select a country")]
    public int CountryId { get; set; }

    [Required(ErrorMessage = "Please select a state")]
    public int StateId { get; set; }

    [Required(ErrorMessage = "Please select a city")]
    public int CityId { get; set; }
    public string? ErrorMessage { get; set; }

    [ValidateNever]
    public List<SelectListItem> Countries { get; set; } = new();
    [ValidateNever]
    public List<SelectListItem> States { get; set; } = new();
    [ValidateNever]
    public List<SelectListItem> Cities { get; set; } = new();

    public List<ProductViewModel> Products { get; set; } = new();

    [ValidateNever]
    public List<int> DeletedProductIds { get; set; } = new();
    
}
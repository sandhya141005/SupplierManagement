using System.ComponentModel.DataAnnotations;
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
    //[Required(ErrorMessage = "Payment methods allowed is required")]
    [ValidateNever]
    public string PaymentMethodsAllowed { get; set; } = "";
    [Required(ErrorMessage = "Select at least one payment method")]
    public List<string> SelectedPaymentMethods { get; set; } = new();
    public string CreatedDate { get; set; } = "";
    public string Country { get; set; } = "";
    public string State { get; set; } = "";
    public string City { get; set; } = "";
    public string ContactNo { get; set; } = "";

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
}

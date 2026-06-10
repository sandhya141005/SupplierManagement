using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace SupplierManagement.Web.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "First Name is required")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Only alphanumeric characters allowed")]
    public string FirstName { get; set; } = "";

    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Only alphanumeric characters allowed")]
    public string LastName { get; set; } = "";

    [Required(ErrorMessage = "Gender is required")]
    public string Gender { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";

    [RegularExpression(@"^$|^\d{10}$", ErrorMessage = "Only numeric and special characters allowed")]
    public string? ContactNo { get; set; }

    [Required(ErrorMessage = "Please select a country")]
    public int CountryId { get; set; }

    [Required(ErrorMessage = "Please select a state")]
    public int StateId { get; set; }

    [Required(ErrorMessage = "Please select a city")]
    public int CityId { get; set; }

    public bool IsAdmin { get; set; }

    public string? ErrorMessage { get; set; }
    public List<SelectListItem> Countries { get; set; } = new();
    public List<SelectListItem> States { get; set; } = new();
    public List<SelectListItem> Cities { get; set; } = new();
}
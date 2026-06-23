using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
public class ProductViewModel
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Product Name is Required")]
    public string ProductName { get; set; } = "";
[ValidateNever]
    public string Category { get; set; } = "";

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
    public int AvailableStock { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Discount cannot be negative")]
    public decimal Discount { get; set; }
    [Required(ErrorMessage = "Product date is required")]
    
   public DateTime? CreatedDate { get; set; }
   
[ValidateNever]
    public int SupplierId { get; set; }
}
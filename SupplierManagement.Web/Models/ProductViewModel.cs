using System.ComponentModel.DataAnnotations;
public class ProductViewModel
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Product Name is Required")]
    public string ProductName { get; set; } = "";

    public string Category { get; set; } = "";

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
    public int AvailableStock { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Discount cannot be negative")]
    public decimal Discount { get; set; }

    public int SupplierId { get; set; }
}
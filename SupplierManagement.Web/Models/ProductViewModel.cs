using System.ComponentModel.DataAnnotations;
public class ProductViewModel
{
    public int ProductId { get; set; }

    [Required(ErrorMessage ="Product Name is Required")]
    public string ProductName { get; set; } = "";

    public string Category { get; set; } = "";
[Required(ErrorMessage ="Product Price is Required")]
    public decimal Price { get; set; }
    [Required(ErrorMessage ="Stock is Required")]
    public int AvailableStock { get; set; }

    public decimal Discount { get; set; }

    public int SupplierId { get; set; }
}
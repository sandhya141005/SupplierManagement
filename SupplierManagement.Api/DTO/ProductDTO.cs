public class ProductDTO
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = "";

    public string Category { get; set; } = "";

    public decimal Price { get; set; }

    public decimal Discount { get; set; }

    public int AvailableStock { get; set; }

    public int SupplierId { get; set; }
    //public DateTime CreatedDate { get; set; }
    public string CreatedDate { get; set; } = ""; 
}
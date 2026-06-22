namespace SupplierManagement.Web.Models;

public class CartItemViewModel
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public string SupplierName { get; set; } = "";
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public int Quantity { get; set; }
    public int AvailableStock { get; set; }
    public decimal LineTotal => (Price - Discount) * Quantity;
}
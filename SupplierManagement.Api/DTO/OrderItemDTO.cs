namespace SupplierManagement.Api.DTO;
public class OrderItemDTO
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal { get; set; }
}
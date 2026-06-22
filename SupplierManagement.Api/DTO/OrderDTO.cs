namespace SupplierManagement.Api.DTO;
public class OrderDTO
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = "";
    public string OrderDate { get; set; } = "";
    public decimal TotalAmount { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; } = new();
}
namespace SupplierManagement.Api.DTO;
public class PlaceOrderDTO
{
    public int UserId { get; set; }
    public List<OrderItemDTO> Items { get; set; } = new();
}
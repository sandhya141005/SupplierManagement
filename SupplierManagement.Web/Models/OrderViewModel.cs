namespace SupplierManagement.Web.Models;

public class OrderViewModel
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = "";
    public string OrderDate { get; set; } = "";
    public decimal TotalAmount { get; set; }
    public List<OrderItemViewModel> OrderItems { get; set; } = new();
}
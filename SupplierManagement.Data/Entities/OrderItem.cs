namespace SupplierManagement.Data.Entities;
public class OrderItem
{
    public int OrderItemId{get;set;}
    public int OrderId{get;set;}
    public string ProductName { get; set; } = "";
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal { get; set; }
    public Order? Order { get; set; }
}
using Microsoft.Identity.Client;

namespace SupplierManagement.Data.Entities;
public class Order
{
    public int OrderId{get; set;}
    public string OrderNumber{ get; set;}="";
    public DateTime OrderDate{get; set;}
    public int UserId{get; set;}
    public decimal TotalAmount{get;set;}
    public List<OrderItem> OrderItems {get; set;}=new();
}
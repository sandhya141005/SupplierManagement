using SupplierManagement.Data.Entities;
namespace SupplierManagement.Business.Interfaces;

public interface IOrderService
{
    Order PlaceOrder(Order order, List<(int productId, int qty)> items);
    List<Order> GetOrdersByUser(int userId);
    Order? GetOrderById(int orderId);
}
using SupplierManagement.Data.Entities;
namespace SupplierManagement.Data.Interfaces;

public interface IOrderRepository
{
    Order PlaceOrder(Order order, List<(int productId, int qty)> items);
    List<Order> GetOrdersByUser(int userId);
    Order? GetOrderById(int orderId);
}
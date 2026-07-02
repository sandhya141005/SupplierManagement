using SupplierManagement.Data.Entities;
using SupplierManagement.Data.DTO;
namespace SupplierManagement.Data.Interfaces;

public interface IOrderRepository
{
    Order PlaceOrder(Order order, List<(int productId, int qty)> items);
    List<Order> GetOrdersByUser(int userId);
    Order? GetOrderById(int orderId);
    Task<List<OrderSummaryDTO>> GetOrderSummaryAsync();

}
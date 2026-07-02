namespace SupplierManagement.Business.Interfaces
{
    public interface IOrderSkill
    {
        Task<List<OrderSummaryDTO>> GetOrderSummaryAsync();
    }
}
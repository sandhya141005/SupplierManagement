using SupplierManagement.Data.DTO;
namespace SupplierManagement.Data.Interfaces
{
    public interface IRevenueRepository
    {
        Task<List<SupplierRevenueDTO>> GetTopSuppliersAsync(int count);
    }
}
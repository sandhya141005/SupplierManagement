using SupplierManagement.Data.DTO;

namespace SupplierManagement.Business.Interfaces
{
    public interface IRevenueSkill
    {
        Task<List<SupplierRevenueDTO>> GetTopSuppliersAsync(int count=5);
    }
}
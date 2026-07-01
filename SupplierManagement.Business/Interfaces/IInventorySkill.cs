using SupplierManagement.Data.DTO;
namespace SupplierManagement.Business.Interfaces
{
   
    public interface IInventorySkill
    {
        Task<List<ProductInventoryDTO>> GetStocksAsync();
    }
}
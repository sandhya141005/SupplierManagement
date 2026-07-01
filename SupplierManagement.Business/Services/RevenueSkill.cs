using Microsoft.EntityFrameworkCore;
using SupplierManagement.Data.DTO;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data;
using SupplierManagement.Data.Interfaces;
using SupplierManagement.Data.Context;
namespace SupplierManagement.Business.Services
{
    public class RevenueSkill : IRevenueSkill
    {
        private readonly IRevenueRepository _repo;
        public RevenueSkill(IRevenueRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<SupplierRevenueDTO>> GetTopSuppliersAsync(int count = 5)
        {

            return await _repo.GetTopSuppliersAsync(count);
        }
    }
}
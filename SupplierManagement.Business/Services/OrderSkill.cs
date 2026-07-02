using Microsoft.EntityFrameworkCore;
using SupplierManagement.Data.DTO;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data;
using SupplierManagement.Data.Interfaces;
using SupplierManagement.Data.Context;
namespace SupplierManagement.Business.Services
{
    public class OrderSkill : IOrderSkill
    {
        private readonly IOrderRepository _repo;
        public OrderSkill(IOrderRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<OrderSummaryDTO>> GetOrderSummaryAsync()
        {
            return await _repo.GetOrderSummaryAsync();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using SupplierManagement.Business.DTO;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data;
using SupplierManagement.Data.Context;
namespace SupplierManagement.Business.Services
{
    public class RevenueSkill : IRevenueSkill
    {
        private readonly AppDbContext _context;
        public RevenueSkill(AppDbContext context)
        {
            _context=context;
        }
        public async Task<List<SupplierRevenueDTO>> GetTopSuppliersAsync(int count = 5)
        {
            var result=await(
                from o in _context.Orders 
                from oi in o.OrderItems
                join p in _context.Products on oi.ProductId equals p.ProductId
                join s in _context.Suppliers on p.SupplierId equals s.SupplierId
                select new{CompanyName=s.CompanyName,OrderId=o.OrderId,LineRevenue=oi.LineTotal})
                .GroupBy(x=>x.CompanyName)
                .Select(g=>new SupplierRevenueDTO
                {
                    CompanyName=g.Key,
                    Revenue=g.Sum(x=>x.LineRevenue),
                    OrderCount=g.Select(x=>x.OrderId).Distinct().Count()
                })
                .OrderByDescending(x=>x.Revenue)
                .Take(count)
                .ToListAsync();
                return result;

            
        }
    }
}
using SupplierManagement.Data.Context;
using SupplierManagement.Data.DTO;
using SupplierManagement.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
public class RevenueRepository : IRevenueRepository
{
    private readonly AppDbContext _context;

    public RevenueRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SupplierRevenueDTO>> GetSuppliersAsync()
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
                .ToListAsync();
                return result;
    }
}
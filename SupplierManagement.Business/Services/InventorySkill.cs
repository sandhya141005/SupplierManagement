
using Microsoft.EntityFrameworkCore;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data.Context;
using SupplierManagement.Data.DTO;
namespace SupplierManagement.Business.Services
{
    public class InventorySkill : IInventorySkill
    {
        private readonly AppDbContext _context;

        public InventorySkill(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductInventoryDTO>> GetStocksAsync()
        {
            var prods = await _context.Products
            .Include(p => p.Supplier)
            .ToListAsync();
            var unitsSoldByProd = await _context.OrderItems
            .GroupBy(oi => oi.ProductId)
            .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(oi => oi.Quantity) })
            .ToListAsync();

            var soldLookup = unitsSoldByProd.ToDictionary(x => x.ProductId, x => x.TotalSold);
            foreach (var kv in soldLookup)
            {
                Console.WriteLine($"{kv.Key} -> {kv.Value}");
            }
            foreach (var p in prods)
            {
                Console.WriteLine(
                    $"{p.ProductId} {p.ProductName} Sold = " +
                    (soldLookup.ContainsKey(p.ProductId)
                        ? soldLookup[p.ProductId]
                        : -1));
            }
            return prods.Select(p => new ProductInventoryDTO
            {

                ProductName = p.ProductName,
                Category = p.Category,
                StockQuantity = p.AvailableStock,
                CompanyName = p.Supplier.CompanyName,
                TotalUnitsSold = soldLookup.ContainsKey(p.ProductId) ? soldLookup[p.ProductId] : 0

            }).ToList();
        }
    }
}
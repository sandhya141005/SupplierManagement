using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplierManagement.Api.DTO;
using SupplierManagement.Data.Context;

namespace SupplierManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly AppDbContext _context;
    public AnalyticsController(AppDbContext context) => _context = context;

    [HttpGet("supplier-revenue")]
    public IActionResult GetSupplierRevenue()
    {
        var result=_context.OrderItems.Join(_context.Products,oi=>oi.ProductId,p=>p.ProductId,
        (oi,p)=>new{oi.LineTotal,p.SupplierId})
        .Join(_context.Suppliers,x=>x.SupplierId,s=>s.SupplierId,
        (x,s)=>new{x.LineTotal,s.CompanyName})
        .GroupBy(x=>x.CompanyName)
        .Select(g=>new SupplierRevenueDTO
        {
            SupplierName=g.Key,TotalRevenue=g.Sum(x=>x.LineTotal),TotalOrders=g.Count()
        })
        .OrderByDescending(x=>x.TotalRevenue)
        .ToList();

        return Ok(result);

    }
     [HttpGet("country-sales")]
    public IActionResult GetCountrySales()
    {
        var result = _context.OrderItems
            .Join(_context.Products,oi => oi.ProductId,p => p.ProductId,
            (oi, p) => new { oi.LineTotal, p.SupplierId })
            .Join(_context.Suppliers,x => x.SupplierId,s => s.SupplierId,
            (x, s) => new { x.LineTotal, s.CountryId })
            .Join(_context.Countries,x => x.CountryId,c => c.CountryId,
            (x, c) => new { x.LineTotal, c.CountryName })
            .GroupBy(x => x.CountryName)
            .Select(g => new CountrySalesDTO
            {
                CountryName = g.Key,
                TotalRevenue = g.Sum(x => x.LineTotal),
                TotalOrders = g.Count()
            })
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();

        return Ok(result);
    }
}
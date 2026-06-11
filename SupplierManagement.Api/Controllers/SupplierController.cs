using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SupplierManagement.Api.DTO;
using SupplierManagement.Business.Interfaces;

namespace SupplierManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _service;
    public SupplierController(ISupplierService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var suppliers = _service.GetAll().Select(s => new SupplierDTO
        {
            SupplierId = s.SupplierId,
            CompanyName = s.CompanyName,
            TotalProducts = s.TotalProducts,
            CatalogType = s.CatalogType,
            PaymentMethodsAllowed = s.PaymentMethodsAllowed,
            CreatedDate = s.CreatedDate.ToString("dd-MMM-yyyy"),
            Country = s.Country?.CountryName ?? "",
            State = s.State?.StateName ?? "",
            City = s.City?.CityName ?? "",
            ContactNo = s.ContactNo
        }).ToList();

        return Ok(suppliers);
    }
}
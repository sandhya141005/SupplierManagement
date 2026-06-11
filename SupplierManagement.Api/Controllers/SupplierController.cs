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
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _service.Delete(id);
            return Ok("Deleted successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var supp = _service.GetById(id);
        if (supp == null)
            return NotFound("Supplier not found");
        var supplierDTO = new SupplierDTO
        {
            SupplierId = supp.SupplierId,
            CompanyName = supp.CompanyName,
            TotalProducts = supp.TotalProducts,
            CatalogType = supp.CatalogType,
            PaymentMethodsAllowed = supp.PaymentMethodsAllowed,
            CreatedDate = supp.CreatedDate.ToString("dd-MMM-yyyy"),
            ContactNo = supp.ContactNo
        };
        return Ok(supplierDTO);
    }
    [HttpPut("{id}")]
    public IActionResult Edit(int id, SupplierDTO dto)
    {
        try
        {
            var supp = _service.GetById(id);
            if (supp == null)
                return NotFound("Supplier not found");
            supp.CompanyName = dto.CompanyName;
            supp.TotalProducts = dto.TotalProducts;
            supp.CatalogType = dto.CatalogType;
            supp.PaymentMethodsAllowed = dto.PaymentMethodsAllowed;
            supp.ContactNo = dto.ContactNo; 
            _service.Edit(supp);
            return Ok("Edited successfully");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
}
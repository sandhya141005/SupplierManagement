using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SupplierManagement.Api.DTO;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data.Entities;
using AutoMapper;
namespace SupplierManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _service;
    private readonly IMapper _mapper;
    public SupplierController(ISupplierService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        /*var suppliers = _service.GetAll().Select(s => new SupplierDTO
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
        */
        var suppliers = _service.GetAll();
        var dtos = _mapper.Map<List<SupplierDTO>>(suppliers);
        return Ok(dtos);
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
        /*var supp = _service.GetById(id);
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
        return Ok(supplierDTO);*/
        var supp = _service.GetById(id);
        if (supp == null)
            return NotFound("Supplier not found");
        return Ok(_mapper.Map<SupplierDTO>(supp));
    }
    [HttpPut("{id}")]
    public IActionResult Edit(int id, SupplierDTO dto)
    {
        /*try
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        */
        try
        {
            var supp = _service.GetById(id);
            if (supp == null)
                return NotFound("Supplier not found");
            _mapper.Map(dto, supp);
            _service.Edit(supp);
            return Ok("Edited successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    // [HttpPost]
    /*public IActionResult Add(SupplierDTO dto)
    {
        try
        {
            Supplier supplier = new Supplier;
            {
                CompanyName = dto.CompanyName,
            TotalProducts = dto.TotalProducts,
            CatalogType = dto.CatalogType,
            PaymentMethodsAllowed = dto.PaymentMethodsAllowed,
            CreatedDate = DateTime.Now,
            CountryId = dto.CountryId,
            StateId = dto.StateId,
            CityId = dto.CityId,
            ContactNo = dto.ContactNo


            }
            _service.Add(supplier);
            return Ok("Added successfully");
        }
    }*/
    [HttpPost]
    [HttpPost]
    public IActionResult Add(SupplierDTO dto)
    {
        try
        {
            var supplier = _mapper.Map<Supplier>(dto);
            if (dto.Products != null && dto.Products.Count > 0)
            {
                supplier.Products = _mapper.Map<List<Product>>(dto.Products);
                foreach (var p in supplier.Products)
                {
                    if (string.IsNullOrWhiteSpace(p.Category))
                        p.Category = dto.CatalogType;
                    p.CreatedDate = DateTime.Now;
                }
            }

            _service.Add(supplier);
            return Ok("Supplier Added Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
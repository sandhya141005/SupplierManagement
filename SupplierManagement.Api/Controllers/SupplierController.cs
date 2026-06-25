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

        var supp = _service.GetById(id);
        if (supp == null)
            return NotFound("Supplier not found");
        return Ok(_mapper.Map<SupplierDTO>(supp));
    }
    [HttpPut("{id}")]
    public IActionResult Edit(int id, SupplierDTO dto)
    {
        try
        {
            var supp = _service.GetById(id);
            if (supp == null)
                return NotFound("Supplier not found");

            _mapper.Map(dto, supp);


            if (dto.DeletedProductIds != null && dto.DeletedProductIds.Count > 0)
            {
                var toRemove = supp.Products
                    .Where(p => dto.DeletedProductIds.Contains(p.ProductId))
                    .ToList();

                foreach (var p in toRemove)
                {
                    supp.Products.Remove(p);
                }
            }

            if (dto.Products != null)
            {
                foreach (var pDto in dto.Products)
                {
                    if (pDto.ProductId > 0)
                    {
                        var existing = supp.Products.FirstOrDefault(p => p.ProductId == pDto.ProductId);
                        if (existing != null)
                        {
                            _mapper.Map(pDto, existing);
                        }
                    }
                    else
                    {
                        var newProduct = _mapper.Map<Product>(pDto);
                        if (string.IsNullOrWhiteSpace(newProduct.Category))
                            newProduct.Category = dto.CatalogType;
                        newProduct.SupplierId = supp.SupplierId;
                        supp.Products.Add(newProduct);
                    }
                }
            }

            _service.Edit(supp);
            return Ok("Edited successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

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


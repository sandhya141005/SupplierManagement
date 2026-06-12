using AutoMapper;
using SupplierManagement.Web.Models;
//using SupplierManagement.Api.DTO;
namespace SupplierManagement.Web.Mappings;

public class WebMappingProfile : Profile
{
    public WebMappingProfile()
    {
        // SupplierViewModel ↔ SupplierDTO (they have same property names)
        //CreateMap<SupplierViewModel, SupplierDTO>().ReverseMap();
    }
}
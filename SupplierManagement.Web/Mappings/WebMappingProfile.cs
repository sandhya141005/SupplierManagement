using AutoMapper;
using SupplierManagement.Web.Models;
using SupplierManagement.Web.MVCDTO;
//using SupplierManagement.Api.DTO;
namespace SupplierManagement.Web.Mappings;

public class WebMappingProfile : Profile
{
    public WebMappingProfile()
    {
        CreateMap<ProductViewModel, ProductDTO>();

        CreateMap<SupplierViewModel, SupplierDTO>().ForMember(dest => dest.CreatedDate,opt => opt.Ignore());
    }
}
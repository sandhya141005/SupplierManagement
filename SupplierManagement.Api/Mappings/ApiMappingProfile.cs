using AutoMapper;
using SupplierManagement.Api.DTO;
using SupplierManagement.Data.Entities;
namespace SupplierManagement.Api.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<RegisterDTO, User>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.IsAdmin ? "Admin" : "User"));

        CreateMap<User, UserDTO>();

        CreateMap<SupplierDTO, Supplier>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src =>
                DateTime.ParseExact(src.CreatedDate, "dd-MMM-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.Products, opt => opt.Ignore());

        CreateMap<Supplier, SupplierDTO>()
            .ForMember(dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.CreatedDate.ToString("dd-MMM-yyyy")))
            .ForMember(dest => dest.DeletedProductIds, opt => opt.Ignore());

        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.CreatedDate.ToString("dd-MMM-yyyy")));

        CreateMap<ProductDTO, Product>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.CreatedDate)
                    ? DateTime.Now
                    : DateTime.ParseExact(src.CreatedDate, "dd-MMM-yyyy",
                        System.Globalization.CultureInfo.InvariantCulture)));
        
    }
}
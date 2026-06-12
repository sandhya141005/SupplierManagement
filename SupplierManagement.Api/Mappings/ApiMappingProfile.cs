using AutoMapper;
using SupplierManagement.Api.DTO;
using SupplierManagement.Data.Entities;
namespace SupplierManagement.Api.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        //check necessity of these mappings
        //RegisterDTO,User
        CreateMap<RegisterDTO, User>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.IsAdmin ? "Admin" : "User"));
        //User.UserDTO
        CreateMap<User, UserDTO>();
        //SuuplierDTO,Supplier
        CreateMap<SupplierDTO, Supplier>()
             .ForMember(dest => dest.CreatedDate,
                 opt => opt.MapFrom(src => DateTime.Now))
             .ForMember(dest => dest.Country, opt => opt.Ignore())
             .ForMember(dest => dest.State, opt => opt.Ignore())
             .ForMember(dest => dest.City, opt => opt.Ignore())
             .ForMember(dest => dest.Products, opt => opt.Ignore());

        // Supplier,SupplierDTO
        CreateMap<Supplier, SupplierDTO>()
            .ForMember(dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.CreatedDate.ToString("dd-MMM-yyyy")))
            .ForMember(dest => dest.Country,
                opt => opt.MapFrom(src => src.Country != null ? src.Country.CountryName : ""))
            .ForMember(dest => dest.State,
                opt => opt.MapFrom(src => src.State != null ? src.State.StateName : ""))
            .ForMember(dest => dest.City,
                opt => opt.MapFrom(src => src.City != null ? src.City.CityName : ""));
    }
}

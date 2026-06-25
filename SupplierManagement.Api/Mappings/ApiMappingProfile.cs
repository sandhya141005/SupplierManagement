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
       
        CreateMap<SupplierDTO, Supplier>().ForMember(dest => dest.Products, opt => opt.Ignore());
        CreateMap<Supplier, SupplierDTO>()
            .ForMember(dest => dest.DeletedProductIds, opt => opt.Ignore());
        CreateMap<Product, ProductDTO>();

        CreateMap<ProductDTO, Product>();
        CreateMap<CartItem, CartItemDTO>().ReverseMap();
       
        CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.OrderDate,
                opt => opt.MapFrom(src => src.OrderDate.ToString("dd-MMM-yyyy HH:mm")))
            .ForMember(dest => dest.OrderItems,
                opt => opt.MapFrom(src => src.OrderItems));

        CreateMap<OrderDTO, Order>()
            .ForMember(dest => dest.OrderDate,
                opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

        
        CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
        CreateMap<PlaceOrderDTO, Order>()
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.OrderNumber,
                opt => opt.MapFrom(src => "ORD-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()))
            .ForMember(dest => dest.TotalAmount,
                opt => opt.MapFrom(src => src.Items.Sum(i => (i.Price - i.Discount) * i.Quantity)))
            .ForMember(dest => dest.OrderItems,
                opt => opt.MapFrom(src => src.Items));

       
        CreateMap<OrderItemDTO, OrderItem>()
            .ForMember(dest => dest.LineTotal,
                opt => opt.MapFrom(src => (src.Price - src.Discount) * src.Quantity));
    }
}
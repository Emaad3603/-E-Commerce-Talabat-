using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using UserAddress = Talabat.Core.Entities.Identity.Address;
using OrderAddress = Talabat.Core.Entities.Order.Address;
using Talabat.Core.Entities.Order;

namespace Talabat.APIs.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDTO>()
                     .ForMember(D => D.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
                     .ForMember(D => D.ProductType, O => O.MapFrom(S => S.ProductType.Name))
                     .ForMember(D => D.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<UserAddress, AddressDto>().ReverseMap();
            CreateMap<OrderAddress,AddressDto>().ReverseMap()
                .ForMember(d=>d.FirstName , O=> O.MapFrom(s=>s.FirstName))
                .ForMember(d=>d.LastName,O=>O.MapFrom(s=>s.LastName));

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderITemPictureUrlResolver>());

            CreateMap<CustomerBasket, CustomerBasketDTO>().ReverseMap();
        }
    }
}

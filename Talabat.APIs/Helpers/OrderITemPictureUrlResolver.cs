using AutoMapper;
using Talabat.Core.Entities.Order;

namespace Talabat.APIs.Helpers
{
    public class OrderITemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {

        private readonly IConfiguration _configuration;

        public OrderITemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {

            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_configuration["BaseUrl"]}/{source.Product.PictureUrl}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

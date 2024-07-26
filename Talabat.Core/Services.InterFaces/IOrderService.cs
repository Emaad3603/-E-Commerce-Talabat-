using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Services.InterFaces
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail , string BasketId , int DeliveryMethodId , Address shippingAddress );

        Task<IReadOnlyList<Order?>> GetOrderForSpecifecUserAsync(string BuyerEmail);

        Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail, int OrderId);



    }
}

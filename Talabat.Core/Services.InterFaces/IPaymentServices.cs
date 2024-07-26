using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Services.InterFaces
{
    public interface IPaymentServices
    {
       Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId);

       Task<Order> UpdatePaymentIntentToSuccedOrFailed(string paymentIntentId, bool flag);
    }
}

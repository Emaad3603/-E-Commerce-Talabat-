using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;
using Talabat.Core.RepositoriesInterFaces;
using Talabat.Core.Services.InterFaces;
using Talabat.Core.Specifications.OrderSpecs;
using Order = Talabat.Core.Entities.Order.Order;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentServices
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork ,
            IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            //Get Basket 
            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket is null) { return null; }

            //total price 

            if(basket.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    if(item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }
           //SubTotal 
          var subTotal =  basket.Items.Sum( i => i.Price * i.Quantity );

            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod =   await  _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }

            //Call Stripe 

            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];

            var service = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if(string.IsNullOrEmpty(basket.PaymentIntentID))
            {
                //Create new paymentintentid 
                var option = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" },

                };
                paymentIntent = await service.CreateAsync(option);
                basket.PaymentIntentID = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                //update paymentintentid
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100),
                };
                paymentIntent = await  service.UpdateAsync(basket.PaymentIntentID, options);
                basket.PaymentIntentID = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            //Return Basket Included PAymentIntentID and Client Secret 


            await _basketRepository.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<Order> UpdatePaymentIntentToSuccedOrFailed(string paymentIntentId, bool flag)
        {
            var spec = new OrderWithPaymentIntentSpecifications(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            if(flag)
            {
                order.Statues = OrderStatues.PaymentReceived;
            }
            else
            {
                order.Statues = OrderStatues.PaymentFailed;
            }

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
 
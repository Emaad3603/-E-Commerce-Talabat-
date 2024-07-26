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

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentServices _paymentServices;

        public OrderService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentServices paymentServices)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentServices = paymentServices;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string BasketId, int DeliveryMethodId, Address shippingAddress)
        {
            var basket = await _basketRepository.GetBasketAsync(BasketId);

            var OrderItem = new List<OrderItem>();

            if (basket?.Items.Count() > 0)
            {
                foreach(var item in basket.Items)
                {
                    var product =   await  _unitOfWork.Repository<Product>().GetAsync(item.Id);

                    var productItemOrder = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrder, item.Price, item.Quantity);

                    OrderItem.Add(orderItem);


                }
            }

            var subTotal = OrderItem.Sum(OI => OI.Price * OI.Quantity);

            var deliveryMethod = await  _unitOfWork.Repository<DeliveryMethod>().GetAsync(DeliveryMethodId);

            //Check if payment intnent id exist for another order 
            var spec = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentID);
            var ExOrder = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);

            if(ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                //Update PAyment intent id with amount of basket if changed 
                basket = await _paymentServices.CreateOrUpdatePaymentIntent(BasketId);
            }

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, OrderItem, subTotal ,basket.PaymentIntentID);
        
           //ORder Saved Locally 
            await  _unitOfWork.Repository<Order>().AddAsync(order);

            var result =   await _unitOfWork.CompleteAsync();

            if(result <= 0)
            {
                return null;
            }
            return order;
           
             
        }

        public async Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail, int OrderId)
        {
            var spec = new OrderSpecifications(buyerEmail,OrderId);

            var order = await  _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);

            if (order is null) return null;
            return order;
        }

        public async Task<IReadOnlyList<Order?>> GetOrderForSpecifecUserAsync(string BuyerEmail)
        {
            var spec = new OrderSpecifications(BuyerEmail);
            var orders =  await  _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            return orders;
        }
    }
}

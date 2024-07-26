using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order
{
    public class Order  : BaseEntity
    {
        public Order()
        {
             
        }

        public Order(
            string buyerEmail,
            Address shippingAddress,
            DeliveryMethod deliveryMethod,
            ICollection<OrderItem> item, 
            decimal subTotal,
            string paymentIntentID
            )
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Item = item;
            SubTotal = subTotal;
            PaymentIntenId = paymentIntentID;
        }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public OrderStatues Statues { get; set; } = OrderStatues.Pending;

        public Address ShippingAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public ICollection<OrderItem> Item { get; set; } = new HashSet<OrderItem>();

        public decimal SubTotal { get; set; }

        public decimal GetTotal ()=> SubTotal + DeliveryMethod.Cost;

        public string PaymentIntenId { get; set; }

    }
}

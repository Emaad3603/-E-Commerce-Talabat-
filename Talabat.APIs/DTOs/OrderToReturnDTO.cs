using Talabat.Core.Entities.Order;

namespace Talabat.APIs.DTOs
{
    public class OrderToReturnDTO
    {
        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public string Statues { get; set; }

        public Address ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; }

        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemDTO> Item { get; set; } = new HashSet<OrderItemDTO>();

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public string PaymentIntenId { get; set; } = string.Empty;

    }
}

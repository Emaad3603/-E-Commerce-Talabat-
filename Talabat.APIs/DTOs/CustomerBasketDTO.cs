using Talabat.Core.Entities;

namespace Talabat.APIs.DTOs
{
    public class CustomerBasketDTO
    {
        public string Id { get; set; }

        public List<BasketItem> Items { get; set; }

        public int? DeliveryMethodId { get; set; }

        public string? PaymentIntentID { get; set; }

        public string? ClientSecret { get; set; }
    }
}

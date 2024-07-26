using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order
{
    public class OrderItemDTO
    {
        

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string PictureUrl { get; set; }
    }
}

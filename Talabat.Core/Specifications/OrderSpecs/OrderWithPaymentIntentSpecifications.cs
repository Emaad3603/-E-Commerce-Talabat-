﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Specifications.OrderSpecs
{
    public class OrderWithPaymentIntentSpecifications : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpecifications(string paymentIntentId): base (o=>o.PaymentIntenId == paymentIntentId)
        {
            
        }
    }
}
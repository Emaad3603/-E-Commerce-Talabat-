﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product>
    {
        public ProductWithCountSpecifications(ProductSpecParams productSpec)
            : base((P =>
            (!productSpec.BrandId.HasValue || P.ProductBrandId == productSpec.BrandId)
            &&
            (!productSpec.TypeId.HasValue || P.ProductTypeId == productSpec.TypeId)
           ))
        {
            
        }
    }
}

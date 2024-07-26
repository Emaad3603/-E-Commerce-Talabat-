using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductWithBrandAndCategorySpecification : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecification(ProductSpecParams productSpec) 
            : base(P => 
            (string.IsNullOrEmpty(productSpec.Search)||P.Name.ToLower().Contains(productSpec.Search))
            &&
            (!productSpec.BrandId.HasValue || P.ProductBrandId == productSpec.BrandId) 
            && 
            (!productSpec.TypeId.HasValue || P.ProductTypeId ==productSpec.TypeId)
            ) 
        {
             Includes.Add(P => P.ProductBrand);
             Includes.Add(P => P.ProductType);
            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch(productSpec.Sort)
                {
                    case "priceAsc": 
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P=>P.Name); 
                        break;
                }

            }

            else
            {
                AddOrderBy(P => P.Name);  
            }


            //total 1000
            //Page index = 9 
            //page size = 50 

            ApplyPagination(productSpec.PageSize * (productSpec.PageIndex -1 ) , productSpec.PageSize);
        }


        public ProductWithBrandAndCategorySpecification( int Id)
            : base(P=>P.Id == Id) 
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}

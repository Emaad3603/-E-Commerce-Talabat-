using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductSpecParams
    {
       
        public string? Sort { get; set; }

        public int? BrandId { get; set; }

        public int? TypeId { get; set; }

        #region Search Prop 

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

        #endregion

        #region Page Size Prop


        private const int MaxPageSize = 5;

        private int pageSize = MaxPageSize; 
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        } 
        #endregion

        public int PageIndex { get; set; } = 1;
    }
}

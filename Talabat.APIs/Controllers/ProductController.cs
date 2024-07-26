using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesInterFaces;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.APIs.Controllers
{
  
    public class ProductsController : BaseAPIController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductType> _categoryRepo;

        public ProductsController(IGenericRepository<Product> productRepo , 
            IMapper mapper 
            , IGenericRepository<ProductBrand> brandRepo ,
            IGenericRepository<ProductType> categoryRepo
            )
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _brandRepo = brandRepo;
            _categoryRepo = categoryRepo;
        }

        #region GetProduct
       // [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProducts ( [FromQuery] ProductSpecParams productSpec)
          
        {
            //var products = await _productRepo.GetAllAsync();
            //   var spec = new BaseSpecifications<Product>();
            var spec = new ProductWithBrandAndCategorySpecification(productSpec);
            var products = await _productRepo.GetAllWithSpecAsync(spec); 
 
            var result =   _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);

            var countSpec = new ProductWithCountSpecifications(productSpec);

            var count = await  _productRepo.GetCountAsync(countSpec);
            // JsonResult result = new JsonResult(products);
            // OkResult result = new OkResult();

            // OkObjectResult result = new OkObjectResult(products);


            return Ok(new Pagination<ProductToReturnDTO>(productSpec.PageIndex , productSpec.PageSize , count , result));
        }
        #endregion


        #region GetProductById
      //  [Authorize]
        [ProducesResponseType(typeof(ProductToReturnDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]

        public async Task<ActionResult<ProductToReturnDTO>> GetProductByid(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecification(id);
            var products = await _productRepo.GetWithSpecAsync(spec);

            if(products is  null)
            {
                return NotFound(new ApiResponse(404));
            }
            var result = _mapper.Map<Product, ProductToReturnDTO>(products);
            return Ok(result);
        }
        #endregion
     //   [Authorize]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandRepo.GetAllAsync();

            return Ok(brands);
        }

      //  [Authorize]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetCategory()
        {
            var categories = await _categoryRepo.GetAllAsync();

            return Ok(categories);
        }
    }
}

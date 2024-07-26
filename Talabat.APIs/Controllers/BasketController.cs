using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesInterFaces;

namespace Talabat.APIs.Controllers
{
   
    public class BasketController : BaseAPIController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository
            ,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet] //GEt : /api/basket/id
        public async Task<ActionResult<CustomerBasket>> GetBasket (string id)
        {
            var basket =  await  _basketRepository.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket (CustomerBasketDTO model)
        {
            var mappedBasket = _mapper.Map<CustomerBasket>(model);
           var basket =  await  _basketRepository.UpdateBasketAsync(mappedBasket);
           if(basket is null) { return BadRequest(new ApiResponse(400)); }
            else { return Ok(basket);}
        }


        [HttpDelete]
        public async Task DeleteBasket (string id)
        {
           await  _basketRepository.DeleteBasketAsync(id);

        }
    }
}

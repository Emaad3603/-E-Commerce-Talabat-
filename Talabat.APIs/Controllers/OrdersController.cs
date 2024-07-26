using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order;
using Talabat.Core.Services.InterFaces;

namespace Talabat.APIs.Controllers
{
  
    public class OrdersController : BaseAPIController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOFWork;

        public OrdersController(
            IOrderService orderService,
            IMapper mapper,
            IUnitOfWork unitOFWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOFWork = unitOFWork;
        }
        [ProducesResponseType(typeof(OrderToReturnDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder (OrderDTO model)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var address = _mapper.Map<AddressDto, Address>(model.ShipToAddress);

            var order =  await _orderService.CreateOrderAsync(buyerEmail,model.BasketId,model.DeliveryMethodId,address);



            if(order is null)
            {
                return BadRequest(new ApiResponse(400, "There is a problem with your order !"));

            }
            else
            {
                var result =  _mapper.Map<Order, OrderToReturnDTO>(order);
                return Ok(result);
            }
        }

        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrderForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders =  await   _orderService.GetOrderForSpecifecUserAsync(buyerEmail);

            if (orders is null) return NotFound(new ApiResponse(404, "there is no orders for u "));

            var result = _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders) ;
            
            return Ok(result);

        }


        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDTO>>  GetOrderNyIdForUser(int id)
        {
             var buyerEmail =   User.FindFirstValue(ClaimTypes.Email);

             var order = await  _orderService.GetOrderByIdForSpecificUserAsync(buyerEmail,id);

            if (order is null) return NotFound(new ApiResponse(404, $"there is not order with this id : {id} : for you !!"));

            var result = _mapper.Map<OrderToReturnDTO>(order) ;
            return Ok(result);
        }


        [HttpGet("DeliveryMethods")]
        
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _unitOFWork.Repository<DeliveryMethod>().GetAllAsync();

            return Ok(deliveryMethods);
        }
    }
}

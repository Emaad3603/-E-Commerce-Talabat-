using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using talabat.Repository.Data;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
   
    public class BuggyController : BaseAPIController
    {
        private readonly StoreDbContext _context;

        public BuggyController(StoreDbContext context)
        {
            _context = context;
        }
        [HttpGet("notfound")]//Get : /api/Buggy/notfound
        public async Task< ActionResult> GetNotFoundRequest()
        {
            var product = await _context.Products.FindAsync(1000);
            if (product is null)
            {
                return NotFound( new ApiResponse (404));
            }
            else
            {
                return Ok(product);
            }

        }

        [HttpGet("servererror")] 
        public async Task<ActionResult> GetServerError()
        {
            var product = await _context.Products.FindAsync(1000);
            var result =  product.ToString(); // will throw Exception [null refernces ]
            return Ok(result);
        }

        [HttpGet("badrequest")]
        public async Task<ActionResult> GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]

        public async Task<IActionResult> GetBadRequest(int? id) //validation error
        {
            return Ok();
        }
        [HttpGet("unauthorized")]
        public async Task<ActionResult> GetUnauthorizedError()//validation error
        {
            return Unauthorized(new ApiResponse(401));
        }
    }
}

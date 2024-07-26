using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services.InterFaces;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class PaymentsController : BaseAPIController
    {
        private readonly IPaymentServices _paymentServices;
        const string endpointSecret = "whsec_308b236321de3d10e11bb2e1d713c45af3122e44d7ec81719509cd91340f6799";


        public PaymentsController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }

        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentServices.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400, "there is a problem with your basket !"));

            return Ok(basket);
        }

        [AllowAnonymous]
        [HttpPost("webhook")]//https://localhost:7211/api/payments/webhook
        
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    await _paymentServices.UpdatePaymentIntentToSuccedOrFailed(paymentIntent.Id, false);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    await _paymentServices.UpdatePaymentIntentToSuccedOrFailed(paymentIntent.Id, true);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

    }

}

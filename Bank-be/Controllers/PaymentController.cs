using Domain.Dtos;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

namespace Bank_be.Controllers
{
    [ApiController]
    [Route("payments")]
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("pay")]
        public ActionResult Pay(PaymentCardDto dto)
        {
            if (dto == null)
                return BadRequest();

            if (!_paymentService.ValidatePaymentCard(dto))
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}

using Domain.Dtos;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

namespace Bank_be.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // tacka 6.
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

        [HttpPost("bank-payment-request")]
        public ActionResult<BankPaymentResponseDto> HandleBankPaymentRequest(BankPaymentRequestDto dto)
        {
            if (dto == null)
                return BadRequest();

            if (!_paymentService.ValidateBankPaymentRequest(dto))
            {
                return NotFound();
            }

            if (!_paymentService.ValidateMerchant(dto.MerchantId))
            {
                return NotFound();
            }

            var response = _paymentService.CreateBankPaymentResponse(dto);

            return Ok(response);
        }
    }
}

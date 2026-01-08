using Domain.Dtos;
using Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace PSP.Controllers
{
    [ApiController]
    [Route("payments")]
    public class PaymentController : Controller
    {
        private readonly IMerchantService _merchantService;
        private readonly IPaymentService _paymentService;

        public PaymentController(IMerchantService merchantService, IPaymentService paymentService)
        {
            _merchantService = merchantService;
            _paymentService = paymentService;
        }

        [HttpPost]
        public ActionResult HandlePaymentInitializationRequest(PaymentInitializationRequestDto dto)
        {
            if (dto == null)
                return BadRequest();

            // checks if MerchantId and MerchantPassword are valid
            if (!_merchantService.VerifyMerchant(dto))
            {
                // TODO : redirect na error url
                return BadRequest();
            }

            // save Request1 (Table1) to database + PspOrderId
            var paymentInitializationRequest = _paymentService.CreatePaymentInitializationRequest(dto);

            var redirectUrl = $"https://localhost:4201/payment/{paymentInitializationRequest.PspOrderId}";

            return Ok(new { redirectUrl = redirectUrl });
        }

        [HttpPost("bank-request")]
        public ActionResult HandleBankPaymentRequest(BankPaymentRequestDto dto)
        {
            if (dto == null)
                return BadRequest();

            // save Request2 (Table2) to database
            var bankPaymentRequest = _paymentService.CreateBankPaymentRequest(dto);

            return Ok();
        }
    }
}

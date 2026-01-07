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
        public ActionResult VerifyMerchant(PaymentInitializationRequestDto dto)
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
            var paymentInitializationRequest = _paymentService.Create(dto);

            // TODO : kreira se link za redirekciju: localhost:psp/psp-order-id

            return Ok();
        }
    }
}

using Domain.Dtos;
using Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace PSP.Controllers
{
    [ApiController]
    [Route("api/bank-payment-requests")]
    public class BankPaymentRequestController : Controller
    {
        private readonly IBankPaymentRequestService _bankPaymentRequestService;

        public BankPaymentRequestController(IBankPaymentRequestService bankPaymentRequestService)
        {
            _bankPaymentRequestService = bankPaymentRequestService;
        }

        [HttpPost("{orderId:guid}")]
        public async Task<ActionResult<BankPaymentResponseDto>> Create([FromRoute] Guid orderId)
        {
            if (orderId == null)
            {
                return BadRequest();
            }

            var createdBankPaymentRequest = await _bankPaymentRequestService.Create(orderId);

            return Ok(createdBankPaymentRequest);
        }

        [HttpPost("qr/{orderId:guid}")]
        public async Task<ActionResult<BankPaymentResponseDto>> CreateQR([FromRoute] Guid orderId)
        {
            if (orderId == null)
            {
                return BadRequest();
            }

            var createdBankPaymentRequest = await _bankPaymentRequestService.CreateQR(orderId);

            return Ok(createdBankPaymentRequest);
        }

        [HttpPost("finalize")]
        public async Task<ActionResult<PaymentFinalizationResponseDto>> FinalizePayment([FromBody] PaymentFinalizationRequestDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var response = await _bankPaymentRequestService.Finalize(request);

            return Ok(response);
        }
    }
}

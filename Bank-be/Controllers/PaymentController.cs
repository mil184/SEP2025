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
        [HttpPost("pay/{orderId:guid}")]
        public async Task<ActionResult<PaymentFinalizationResponseDto>> Pay([FromBody] PaymentCardDto dto, [FromRoute] Guid orderId)
        {
            if (dto == null)
                return BadRequest();

            if (!_paymentService.ValidatePaymentCard(dto))
            {
                return BadRequest();
            }

            try
            {
                var request = _paymentService.Pay(dto, orderId);
                return Ok(await _paymentService.UpdateStatus(request));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            };
        }

        [HttpGet("amount/{orderId:guid}")]
        public ActionResult<double> GetAmount([FromRoute] Guid orderId)
        {
            return Ok(_paymentService.GetAmount(orderId));
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

        [HttpPost("bank-payment-request/qr")]
        public ActionResult<BankPaymentResponseDto> HandleBankPaymentQRRequest(BankPaymentRequestDto dto)
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

            var response = _paymentService.CreateBankPaymentQRResponse(dto);

            return Ok(response);
        }

        [HttpGet("qr/{orderId:guid}")]
        public IActionResult GetQr([FromRoute] Guid orderId)
        {
            var pngBytes = _paymentService.GenerateQrPngForOrder(orderId);

            // returns image/png
            return File(pngBytes, "image/png");
        }

        [HttpPost("qr/validate")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<string?>> ValidateQr([FromForm] QrValidateRequest request)
        {
            var file = request.File;

            if (file == null || file.Length == 0) return Ok(null);
            if (!string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase)) return Ok(null);

            await using var stream = file.OpenReadStream();

            var decodedText = await QrPayload.TryDecodeTextAsync(stream);
            if (decodedText is null) return Ok(null);

            return Ok(QrPayload.IsValidPayload(decodedText) ? decodedText : null);
        }
    }
}

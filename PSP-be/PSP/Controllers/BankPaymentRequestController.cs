using Domain.Models;
using Domain.Service;
using Microsoft.AspNetCore.Mvc;
using PSP.Dtos;

namespace PSP.Controllers
{
    [ApiController]
    [Route("bank-payment-requests")]
    public class BankPaymentRequestController : Controller
    {
        private readonly IBankPaymentRequestService _bankPaymentRequestService;

        public BankPaymentRequestController(IBankPaymentRequestService bankPaymentRequestService)
        {
            _bankPaymentRequestService = bankPaymentRequestService;
        }

        [HttpPost]
        public ActionResult<BankPaymentRequest> Create(BankPaymentRequestDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }

            var bankPaymentRequest = new BankPaymentRequest()
            {
                MerchantId = userDto.MerchantId,
                Amount = userDto.Amount,
                Currency = userDto.Currency,
                Stan = userDto.Stan
            };
            var createdBankPaymentRequest = _bankPaymentRequestService.Create(bankPaymentRequest);

            return CreatedAtAction(nameof(Create), createdBankPaymentRequest);
        }
    }
}

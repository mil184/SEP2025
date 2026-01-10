using Domain.Dtos;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    public class PaymentInitializationRequestController : Controller
    {
        private readonly IPaymentInitializationRequestService _service;

        public PaymentInitializationRequestController(IPaymentInitializationRequestService service)
        {
            _service = service;
        }



        [HttpPost]
        public ActionResult<PaymentInitializationRequest> Create(PaymentInitializationRequestDto dto)
        {
            if (dto == null)
                return BadRequest();

            var created = _service.Create(dto);

            //

            return Ok(created);
        }
    }
}


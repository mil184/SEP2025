using Domain.Dtos;
using Domain.Models;
using Domain.Services;
using Infrastructure.Helpers;
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

        [HttpGet("{id}")]
        public ActionResult<PaymentInitializationRequest> GetById(string id)
        {
            var guid = GuidHelper.GetGuidFromString(id);
            var req = _service.GetById(guid);

            return Ok(req);
        }

        [HttpGet]
        public ActionResult<IEnumerable<PaymentInitializationRequest>> GetAll()
        {
            var reqs = _service.GetAll();
            return Ok(reqs);
        }

        [HttpPost]
        public ActionResult<PaymentInitializationRequest> Create(PaymentInitializationRequestDto dto)
        {
            if (dto == null)
                return BadRequest();


            var created = _service.Create(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var guid = GuidHelper.GetGuidFromString(id);
            var req = _service.GetById(guid);

            _service.Delete(req);
            return Ok("Successfully deleted payment initialization request.");
        }
    }
}


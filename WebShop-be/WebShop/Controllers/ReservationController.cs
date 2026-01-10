using Domain.Dtos;
using Domain.Models;
using Domain.Services;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("reservations")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> GetAll()
        {
            return Ok(_reservationService.GetAll());
        }

        [HttpGet("user/{userId:guid}")]
        public ActionResult<IEnumerable<Reservation>> GetAllByUser(Guid userId)
        {
            var reservations = _reservationService.GetAllByUser(userId);
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public ActionResult<Reservation> GetById(string id)
        {
            var guid = GuidHelper.GetGuidFromString(id);

            var reservation = _reservationService.GetById(guid);
            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentInitializationResponseDto>> Create(ReservationDto reservationDto)
        {
            if (reservationDto == null)
            {
                return BadRequest();
            }

            var reservation = new Reservation() { UserId = reservationDto.UserId, VehicleId = reservationDto.VehicleId, StartDate = reservationDto.StartDate, EndDate = reservationDto.EndDate };
            var paymentInit = await _reservationService.Create(reservation);

            return Ok(paymentInit);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Reservation reservation)
        {
            if (reservation == null)
            {
                return BadRequest();
            }

            var guid = GuidHelper.GetGuidFromString(id);
            reservation.Id = guid;

            _reservationService.Update(reservation);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var guid = GuidHelper.GetGuidFromString(id);

            var reservation = _reservationService.GetById(guid);
            if (reservation == null)
            {
                return NotFound();
            }

            _reservationService.Delete(reservation);
            return NoContent();
        }
    }
}

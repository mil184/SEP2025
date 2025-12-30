using Domain.Dtos;
using Domain.Models;
using Domain.Services;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("vehicles")]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("{id}")]
        public ActionResult<Vehicle> GetById(string id)
        {
            var guid = GuidHelper.GetGuidFromString(id);
            var vehicle = _vehicleService.GetById(guid);

            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Vehicle>> GetAll()
        {
            var vehicles = _vehicleService.GetAll();
            return Ok(vehicles);
        }

        [HttpPost]
        public ActionResult<Vehicle> Create(VehicleDto vehicleDto)
        {
            if (vehicleDto == null)
            {
                return BadRequest();
            }

            var vehicle = new Vehicle()
            {
                // Adjust property names to match your Vehicle model + DTO
                Brand = vehicleDto.Brand,
                Model = vehicleDto.Model,
                Year = vehicleDto.Year,
                PricePerDay = vehicleDto.PricePerDay,
                IsAvailable = vehicleDto.IsAvailable
            };

            var createdVehicle = _vehicleService.Create(vehicle);

            return CreatedAtAction(nameof(GetById), new { id = createdVehicle.Id }, createdVehicle);
        }

        [HttpPut("{id}")]
        public ActionResult Update(string id, VehicleDto vehicleDto)
        {
            if (vehicleDto == null)
            {
                return BadRequest();
            }

            var guid = GuidHelper.GetGuidFromString(id);
            var existing = _vehicleService.GetById(guid);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Brand = vehicleDto.Brand;
            existing.Model = vehicleDto.Model;
            existing.Year = vehicleDto.Year;
            existing.PricePerDay = vehicleDto.PricePerDay;
            existing.IsAvailable = vehicleDto.IsAvailable;

            _vehicleService.Update(existing);
            return Ok("Successfully updated vehicle.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var guid = GuidHelper.GetGuidFromString(id);
            var vehicle = _vehicleService.GetById(guid);

            if (vehicle == null)
            {
                return NotFound();
            }

            _vehicleService.Delete(vehicle);
            return Ok("Successfully deleted vehicle.");
        }
    }
}

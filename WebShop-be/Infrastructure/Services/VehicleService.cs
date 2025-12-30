using Domain.Models;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public Vehicle Create(Vehicle vehicle)
        {
            return _vehicleRepository.Create(vehicle);
        }

        public void Delete(Vehicle vehicle)
        {
            _vehicleRepository.Delete(vehicle);
        }

        public IEnumerable<Vehicle> GetAll()
        {
            return _vehicleRepository.GetAll();
        }

        public Vehicle? GetById(Guid id)
        {
            var vehicle = _vehicleRepository.GetById(id);

            if (vehicle == null)
            {
                throw new ArgumentException("No vehicle found with the specified id.");
            }

            return vehicle;
        }

        public void Update(Vehicle vehicle)
        {
            _vehicleRepository.Update(vehicle);
        }
    }
}

using Domain.Models;

namespace Domain.Services
{
    public interface IVehicleService
    {
        Vehicle Create(Vehicle vehicle);
        void Delete(Vehicle vehicle);
        IEnumerable<Vehicle> GetAll();
        Vehicle? GetById(Guid id);
        void Update(Vehicle vehicle);
    }
}

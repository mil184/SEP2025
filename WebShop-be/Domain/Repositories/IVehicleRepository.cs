using Domain.Models;

namespace Domain.Repositories
{
    public interface IVehicleRepository
    {
        Vehicle Create(Vehicle vehicle);
        void Delete(Vehicle vehicle);
        IEnumerable<Vehicle> GetAll();
        Vehicle? GetById(Guid id);
        void Update(Vehicle vehicle);

    }
}

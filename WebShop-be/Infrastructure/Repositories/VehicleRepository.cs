using Domain.Models;
using Domain.Repositories;
using Infrastructure.DataContext;

namespace Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        public Vehicle Create(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
            return vehicle;
        }

        public void Delete(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
        }

        public IEnumerable<Vehicle> GetAll()
        {
            return _context.Vehicles.ToList();
        }

        public Vehicle? GetById(Guid id)
        {
            return _context.Vehicles.FirstOrDefault(v => v.Id == id);
        }

        public void Update(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();
        }
    }
}

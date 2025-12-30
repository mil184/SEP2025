using Domain.Models;

namespace Domain.Services
{
    public interface IReservationService
    {
        Reservation Create(Reservation reservation);
        void Delete(Reservation reservation);
        IEnumerable<Reservation> GetAll();
        Reservation? GetById(Guid id);
        void Update(Reservation reservation);
    }
}

using Domain.Models;

namespace Domain.Repositories
{
    public interface IReservationRepository
    {
        Reservation Create(Reservation reservation);
        void Delete(Reservation reservation);
        IEnumerable<Reservation> GetAll();
        Reservation? GetById(Guid id);
        void Update(Reservation reservation);
    }
}

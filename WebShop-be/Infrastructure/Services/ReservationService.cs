using Domain.Models;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public Reservation Create(Reservation reservation)
        {
            return _reservationRepository.Create(reservation);
        }

        public void Delete(Reservation reservation)
        {
            _reservationRepository.Delete(reservation);
        }

        public IEnumerable<Reservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }

        public Reservation? GetById(Guid id)
        {
            var reservation = _reservationRepository.GetById(id);

            if (reservation == null)
            {
                throw new ArgumentException("No reservation found with the specified id.");
            }

            return reservation;
        }

        public void Update(Reservation reservation)
        {
            _reservationRepository.Update(reservation);
        }
    }
}


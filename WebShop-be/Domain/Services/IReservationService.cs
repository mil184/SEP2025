using Domain.Dtos;
using Domain.Models;

namespace Domain.Services
{
    public interface IReservationService
    {
        Task<PaymentInitializationResponseDto> Create(Reservation reservation);
        void Delete(Reservation reservation);
        IEnumerable<Reservation> GetAll();
        IEnumerable<Reservation> GetAllByUser(Guid userId);
        Reservation? GetById(Guid id);
        void Update(Reservation reservation);
    }
}

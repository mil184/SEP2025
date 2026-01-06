using Domain.Dtos;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IVehicleService _vehicleService;
        private readonly IPaymentInitializationRequestService _paymentInitializationRequestService;

        public ReservationService(IReservationRepository reservationRepository, IVehicleService vehicleService, IPaymentInitializationRequestService paymentInitializationRequestService)
        {
            _reservationRepository = reservationRepository;
            _vehicleService = vehicleService;
            _paymentInitializationRequestService = paymentInitializationRequestService;
        }

        public Reservation Create(Reservation reservation)
        {

            var paymentInitializationRequest = new PaymentInitializationRequestDto()
            {
                MerchantOrderId = reservation.Id,
                Amount = GetPaymentAmount(reservation),
                Currency = Domain.Enums.Currency.EUR
            };
            _paymentInitializationRequestService.Create(paymentInitializationRequest);

            return _reservationRepository.Create(reservation);
        }

        private double GetPaymentAmount(Reservation reservation)
        {
            var reservationDays = reservation.EndDate.DayNumber - reservation.StartDate.DayNumber;
            double pricePerDay = _vehicleService.GetById(reservation.VehicleId).PricePerDay;
            return reservationDays * pricePerDay;
        }

        public void Delete(Reservation reservation)
        {
            _reservationRepository.Delete(reservation);
        }

        public IEnumerable<Reservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }

        public IEnumerable<Reservation> GetAllByUser(Guid userId)
        {
            return GetAll().Where(x => x.UserId == userId).ToList();
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


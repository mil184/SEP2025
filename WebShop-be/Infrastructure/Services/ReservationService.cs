using Domain.Dtos;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Clients;
using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IVehicleService _vehicleService;
        private readonly IPaymentInitializationRequestService _paymentInitializationRequestService;
        private readonly PspClient _pspClient;

        private readonly string _merchant_id;
        private readonly string _merchant_password;

        public ReservationService(IReservationRepository reservationRepository, IVehicleService vehicleService, IPaymentInitializationRequestService paymentInitializationRequestService, PspClient pspClient, IConfiguration config)
        {
            _reservationRepository = reservationRepository;
            _vehicleService = vehicleService;
            _paymentInitializationRequestService = paymentInitializationRequestService;
            _pspClient = pspClient;
            _merchant_id = config["Payment:MerchantId"]!;
            _merchant_password = config["Payment:MerchantPassword"]!;
        }

        public async Task<PaymentInitializationResponseDto> Create(Reservation reservation)
        {
            var createdReservation = _reservationRepository.Create(reservation);
            var request = new PaymentInitializationRequestDto()
            {
                MerchantId = GuidHelper.GetGuidFromString(_merchant_id),
                MerchantPassword = _merchant_password,
                Amount = GetPaymentAmount(reservation),
                Currency = Domain.Enums.Currency.RSD,
                MerchantOrderId = createdReservation.Id, // used to update status later
                MerchantTimestamp = DateTime.UtcNow
            };
            _paymentInitializationRequestService.Create(request);
            var response = await _pspClient.InitializeAsync(request);

            return response;
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


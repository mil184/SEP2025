using Domain.Models;
using Domain.Repository;
using Domain.Service;

namespace Infrastructure.Service
{
    public class BankPaymentRequestService : IBankPaymentRequestService
    {
        private readonly IBankPaymentRequestRepository _bankPaymentRequestRepository;

        public BankPaymentRequestService(IBankPaymentRequestRepository bankPaymentRequestRepository)
        {
            _bankPaymentRequestRepository = bankPaymentRequestRepository;
        }

        public BankPaymentRequest Create(BankPaymentRequest request)
        {
            return _bankPaymentRequestRepository.Create(request);
        }
    }
}


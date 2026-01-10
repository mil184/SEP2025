using Domain.Models;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    public class BankPaymentRequestService
    {
        private readonly BankPaymentRequestRepository _repository;

        public BankPaymentRequestService(BankPaymentRequestRepository repository)
        {
            _repository = repository;
        }

        public BankPaymentRequest Create(BankPaymentRequest toSave)
        {
            return _repository.CreateBankPaymentRequest(toSave);
        }
    }
}

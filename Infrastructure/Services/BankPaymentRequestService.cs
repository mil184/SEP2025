using Domain.Enums;
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

        public BankPaymentRequest GetBankPaymentRequest(Guid id)
        {
            return _repository.GetBankPaymentRequest(id);
        }

        public BankPaymentRequest UpdateStatus(Guid id, Status status)
        {
            var updated = _repository.UpdateStatus(id, status);
            if (updated == null)
                throw new KeyNotFoundException("Bank payment request not found.");

            return updated;
        }
    }
}

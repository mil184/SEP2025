using Domain.Enums;
using Domain.Models;
using Infrastructure.DataContext;

namespace Infrastructure.Repositories
{
    public class BankPaymentRequestRepository
    {
        private readonly AppDbContext _context;
        public BankPaymentRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public BankPaymentRequest CreateBankPaymentRequest(BankPaymentRequest bankPaymentRequest)
        {
            _context.BankPaymentRequests.Add(bankPaymentRequest);
            _context.SaveChanges();
            return bankPaymentRequest;
        }

        public BankPaymentRequest GetBankPaymentRequest(Guid id)
        {
            return _context.BankPaymentRequests.FirstOrDefault(x => x.Id == id);
        }

        public BankPaymentRequest UpdateStatus(Guid id, Status status)
        {
            var request = _context.BankPaymentRequests.FirstOrDefault(x => x.Id == id);
            if (request == null)
                return null;

            request.Status = status;
            _context.SaveChanges();

            return request;
        }
    }
}

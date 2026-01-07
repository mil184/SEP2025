using Domain.Models;
using Domain.Repository;
using Infrastructure.DataContext;

namespace Infrastructure.Repository
{
    public class BankPaymentRequestRepository : IBankPaymentRequestRepository
    {
        private readonly AppDbContext _context;

        public BankPaymentRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public BankPaymentRequest Create(BankPaymentRequest request)
        {
            _context.BankPaymentRequests.Add(request);
            _context.SaveChanges();
            return request;
        }
    }
}

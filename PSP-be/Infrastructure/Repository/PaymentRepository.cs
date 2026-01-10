using Domain.Models;
using Domain.Repository;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public PaymentInitializationRequest Get(Guid id)
        {
            return _context.PaymentInitializationRequests
                .AsNoTracking()
                .FirstOrDefault(x => x.PspOrderId == id);
        }

        public PaymentInitializationRequest CreatePaymentInitializationRequest(PaymentInitializationRequest request)
        {
            _context.PaymentInitializationRequests.Add(request);
            _context.SaveChanges();
            return request;
        }

        public BankPaymentRequest CreateBankPaymentRequest(BankPaymentRequest request)
        {
            _context.BankPaymentRequests.Add(request);
            _context.SaveChanges();
            return request;
        }
    }
}

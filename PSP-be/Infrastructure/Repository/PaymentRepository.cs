using Domain.Models;
using Domain.Repository;
using Infrastructure.DataContext;

namespace Infrastructure.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public PaymentInitializationRequest Create(PaymentInitializationRequest request)
        {
            _context.PaymentInitializationRequests.Add(request);
            _context.SaveChanges();
            return request;
        }
    }
}

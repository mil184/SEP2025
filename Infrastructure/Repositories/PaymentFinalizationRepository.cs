using Domain.Models;
using Infrastructure.DataContext;

namespace Infrastructure.Repositories
{
    public class PaymentFinalizationRepository
    {
        private readonly AppDbContext _context;
        public PaymentFinalizationRepository(AppDbContext context)
        {
            _context = context;
        }

        public PaymentFinalization CreatePaymentFinalization(PaymentFinalization paymentFinalization)
        {
            _context.PaymentFinalizations.Add(paymentFinalization);
            _context.SaveChanges();
            return paymentFinalization;
        }
    }
}

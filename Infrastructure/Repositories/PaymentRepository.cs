using Domain.Models;
using Infrastructure.DataContext;

namespace Infrastructure.Repository
{
    public class PaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public PaymentCard? GetByPan(string pan)
        {
            return _context.PaymentCards.FirstOrDefault(x => x.Pan == pan);
        }
    }
}

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


    }
}

using Domain.Models;
using Domain.Repository;
using Infrastructure.DataContext;

namespace Infrastructure.Repository
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly AppDbContext _context;

        public MerchantRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public Merchant? GetByMerchantId(Guid id)
        {
            return _context.Merchants.FirstOrDefault(u => u.Id == id);
        }
    }
}

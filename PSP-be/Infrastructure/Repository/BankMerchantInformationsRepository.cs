using Domain.Models;
using Domain.Repository;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class BankMerchantInformationsRepository : IBankMerchantInformationsRepository
    {
        private readonly AppDbContext _context;

        public BankMerchantInformationsRepository(AppDbContext context)
        {
            _context = context;
        }
        public BankMerchantInformation GetByMerchantId(Guid merchantId)
        {
            return _context.BankMerchantInformations
                .AsNoTracking()
                .FirstOrDefault(x => x.MerchantId == merchantId);
        }
    }
}

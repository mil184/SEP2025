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

        public BankPaymentResponse CreateBankPaymentResponse(BankPaymentResponse bankPaymentResponse)
        {
            _context.BankPaymentResponses.Add(bankPaymentResponse);
            _context.SaveChanges();
            return bankPaymentResponse;
        }

        public Merchant? GetByMerchantId(Guid merchantId)
        {
            return _context.Merchants.FirstOrDefault(x => x.MerchantId == merchantId);
        }

    }
}

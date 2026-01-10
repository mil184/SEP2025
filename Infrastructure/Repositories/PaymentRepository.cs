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

        public PaymentCard? GetMatchingCard(string pan, string securityCode, string cardholderName, DateOnly expirationDate)
        {
            return _context.PaymentCards.FirstOrDefault(x =>
                x.Pan == pan &&
                x.SecurityCode == securityCode &&
                x.CardholderName == cardholderName &&
                x.ExpirationDate == expirationDate
            );
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

        public PaymentCard? UpdateCard(PaymentCard updated)
        {
            var card = _context.PaymentCards.FirstOrDefault(x => x.Pan == updated.Pan);
            if (card == null)
                return null;

            card.Amount = updated.Amount;

            _context.SaveChanges();
            return card;
        }
    }
}

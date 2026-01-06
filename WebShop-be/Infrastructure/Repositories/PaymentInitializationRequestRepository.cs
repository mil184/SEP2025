using Domain.Models;
using Domain.Repositories;
using Infrastructure.DataContext;

namespace Infrastructure.Repositories
{
    public class PaymentInitializationRequestRepository : IPaymentInitializationRequestRepository
    {
        private readonly AppDbContext _context;

        public PaymentInitializationRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public PaymentInitializationRequest Create(PaymentInitializationRequest request)
        {
            _context.PaymentInitializationRequests.Add(request);
            _context.SaveChanges();
            return request;
        }

        public void Delete(PaymentInitializationRequest request)
        {
            _context.PaymentInitializationRequests.Remove(request);
            _context.SaveChanges();
        }

        public IEnumerable<PaymentInitializationRequest> GetAll()
        {
            return _context.PaymentInitializationRequests.ToList();
        }

        public PaymentInitializationRequest? GetById(Guid id)
        {
            return _context.PaymentInitializationRequests.FirstOrDefault(x => x.Id == id);
        }

        public void Update(PaymentInitializationRequest request)
        {
            _context.PaymentInitializationRequests.Update(request);
            _context.SaveChanges();
        }
    }
}


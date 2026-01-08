using Infrastructure.Repository;

namespace Infrastructure.Service
{
    public class PaymentService
    {
        private readonly PaymentRepository _repository;
        public PaymentService(PaymentRepository repository)
        {
            _repository = repository;
        }


    }
}

using Brainwave.Core.Data;
using Brainwave.ManagementPayment.Application;
using Brainwave.ManagementPayment.Application;

namespace Brainwave.ManagementPayment.Data.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentContext _context;

        public PaymentRepository(PaymentContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public void AddTransaction(PaymentTransaction transaction)
        {
            _context.PaymentTransactions.Add(transaction);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}

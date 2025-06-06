using Brainwave.Core.Data;
using Brainwave.ManagementPayment.Business;

namespace Brainwave.ManagementPayment.Application
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        void Add(Payment payment);

        void AddTransaction(PaymentTransaction transaction);
    }

}

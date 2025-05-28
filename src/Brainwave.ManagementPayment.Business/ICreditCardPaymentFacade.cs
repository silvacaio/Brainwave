using System.Transactions;

namespace Brainwave.ManagementPayment.Business
{
    public interface ICreditCardPaymentFacade
    {
        Transaction ProcessPayment(Order order, Payment payment);
    }
}

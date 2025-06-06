using Brainwave.ManagementPayment.Application;

namespace Brainwave.ManagementPayment.Business
{
    public interface ICreditCardPaymentFacade
    {
        PaymentTransaction ProcessPayment(Payment payment);
    }
}

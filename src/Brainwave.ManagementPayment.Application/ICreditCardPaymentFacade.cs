using Brainwave.ManagementPayment.Application;

namespace Brainwave.ManagementPayment.Application
{
    public interface ICreditCardPaymentFacade
    {
        PaymentTransaction ProcessPayment(Payment payment);
    }
}

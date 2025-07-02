using Brainwave.ManagementPayment.Application;
using static Brainwave.ManagementPayment.Application.PaymentTransaction;

namespace Brainwave.ManagementPayment.AntiCorruption
{
    public class CreditCardPaymentFacade : ICreditCardPaymentFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        private readonly IConfigurationManager _configManager;

        public CreditCardPaymentFacade(IPayPalGateway payPalGateway, IConfigurationManager configManager)
        {
            _payPalGateway = payPalGateway;
            _configManager = configManager;
        }

        public PaymentTransaction ProcessPayment(Payment payment)
        {
            var apiKey = _configManager.GetValue("apiKey");
            var encriptionKey = _configManager.GetValue("encriptionKey");

            var serviceKey = _payPalGateway.GetPayPalServiceKey(apiKey, encriptionKey);
            var cardHashKey = _payPalGateway.GetCardHashKey(serviceKey, payment.CreditCard.CardNumber);

            var paymentResult = _payPalGateway.CommitTransaction(cardHashKey, payment.EnrollmentId.ToString(), payment.Value);

            // TODO: O gateway de payments que deve retornar o objeto transação

            if (paymentResult)
            {
                return PaymentTransactionFactory.Paid(payment.EnrollmentId, payment.Id, payment.Value);
            }

            return PaymentTransactionFactory.Refused(payment.EnrollmentId, payment.Id, payment.Value);

        }
    }
}

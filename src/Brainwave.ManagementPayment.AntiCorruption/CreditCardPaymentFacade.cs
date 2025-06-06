using Brainwave.ManagementPayment.Application;
using Brainwave.ManagementPayment.Business;

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
            var transacao = new PaymentTransaction
            {
                EnrollmentId = payment.EnrollmentId,
                Value = payment.Value,
                PaymentId = payment.Id
            };

            if (paymentResult)
            {
                transacao.StatusTransaction = StatusTransaction.Paid;
                return transacao;
            }

            transacao.StatusTransaction = StatusTransaction.Refused;
            return transacao;
        }
    }
}

using System;
using Brainwave.ManagementPayment.AntiCorruption;
using Brainwave.ManagementPayment.Application;
using Moq;
using Xunit;

namespace Brainwave.ManagementPayment.Tests.Infrastructure.Payment
{
    public class CreditCardPaymentFacadeTests
    {
        private readonly Mock<IPayPalGateway> _payPalGatewayMock;
        private readonly Mock<IConfigurationManager> _configManagerMock;
        private readonly CreditCardPaymentFacade _facade;

        public CreditCardPaymentFacadeTests()
        {
            _payPalGatewayMock = new Mock<IPayPalGateway>();
            _configManagerMock = new Mock<IConfigurationManager>();

            _facade = new CreditCardPaymentFacade(_payPalGatewayMock.Object, _configManagerMock.Object);
        }

        [Fact(DisplayName = "Should return a paid transaction when CommitTransaction returns true")]
        public void ProcessPayment_ShouldReturnPaidTransaction_WhenCommitIsTrue()
        {
            // Arrange
            var payment = CreateValidPayment();

            _configManagerMock.Setup(c => c.GetValue("apiKey")).Returns("api-key");
            _configManagerMock.Setup(c => c.GetValue("encriptionKey")).Returns("encryption-key");

            _payPalGatewayMock.Setup(g => g.GetPayPalServiceKey("api-key", "encryption-key"))
                .Returns("service-key");

            _payPalGatewayMock.Setup(g => g.GetCardHashKey("service-key", payment.CreditCard.CardNumber))
                .Returns("card-hash-key");

            _payPalGatewayMock.Setup(g => g.CommitTransaction("card-hash-key", payment.EnrollmentId.ToString(), payment.Value))
                .Returns(true);

            // Act
            var transaction = _facade.ProcessPayment(payment);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(payment.EnrollmentId, transaction.EnrollmentId);
            Assert.Equal(payment.Id, transaction.PaymentId);
            Assert.Equal(payment.Value, transaction.Value);
            Assert.Equal(StatusTransaction.Paid, transaction.StatusTransaction);
        }

        [Fact(DisplayName = "Should return a refused transaction when CommitTransaction returns false")]
        public void ProcessPayment_ShouldReturnRefusedTransaction_WhenCommitIsFalse()
        {
            // Arrange
            var payment = CreateValidPayment();

            _configManagerMock.Setup(c => c.GetValue("apiKey")).Returns("api-key");
            _configManagerMock.Setup(c => c.GetValue("encriptionKey")).Returns("encryption-key");

            _payPalGatewayMock.Setup(g => g.GetPayPalServiceKey("api-key", "encryption-key"))
                .Returns("service-key");

            _payPalGatewayMock.Setup(g => g.GetCardHashKey("service-key", payment.CreditCard.CardNumber))
                .Returns("card-hash-key");

            _payPalGatewayMock.Setup(g => g.CommitTransaction("card-hash-key", payment.EnrollmentId.ToString(), payment.Value))
                .Returns(false);

            // Act
            var transaction = _facade.ProcessPayment(payment);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(payment.EnrollmentId, transaction.EnrollmentId);
            Assert.Equal(payment.Id, transaction.PaymentId);
            Assert.Equal(payment.Value, transaction.Value);
            Assert.Equal(StatusTransaction.Refused, transaction.StatusTransaction);
        }

        private Application.Payment CreateValidPayment()
        {
            var creditCard = new Application.ValueObjects.CreditCard(
                "4111111111111111",
                "John Doe",
                DateTime.UtcNow.AddMonths(1),
                "123");

            var payment = new Application.Payment(Guid.NewGuid(), 100m);
            payment.AddCreditCard(creditCard);

            return payment;
        }
    }
}

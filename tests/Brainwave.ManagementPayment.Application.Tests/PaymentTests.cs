using Brainwave.ManagementPayment.Application;
using Brainwave.ManagementPayment.Application.ValueObjects;
using System;
using Xunit;

namespace Brainwave.ManagementPayment.Tests.Domain
{
    public class PaymentTests
    {
        [Fact(DisplayName = "Should initialize payment correctly")]
        [Trait("Payment", "ManagementPayment - Payment")]
        public void Payment_Constructor_ShouldInitializeCorrectly()
        {
            // Arrange
            var enrollmentId = Guid.NewGuid();
            var value = 199.99m;

            // Act
            var payment = new Payment(enrollmentId, value);

            // Assert
            Assert.Equal(enrollmentId, payment.EnrollmentId);
            Assert.Equal(value, payment.Value);
            Assert.Null(payment.CreditCard);
            Assert.Null(payment.Transaction);
        }

        [Fact(DisplayName = "Should add valid credit card to payment")]
        [Trait("Payment", "ManagementPayment - Payment")]
        public void AddCreditCard_ShouldAdd_WhenCardIsValid()
        {
            // Arrange
            var payment = new Payment(Guid.NewGuid(), 100);
            var creditCard = new CreditCard("4111111111111111", "John Doe", DateTime.Today.AddDays(10), "123");

            // Act
            payment.AddCreditCard(creditCard);

            // Assert
            Assert.NotNull(payment.CreditCard);
            Assert.Equal("John Doe", payment.CreditCard.CardHolderName);
        }

        [Fact(DisplayName = "Should not add null credit card")]
        [Trait("Payment", "ManagementPayment - Payment")]
        public void AddCreditCard_ShouldNotAdd_WhenCardIsNull()
        {
            // Arrange
            var payment = new Payment(Guid.NewGuid(), 100);

            // Act
            payment.AddCreditCard(null!);

            // Assert
            Assert.Null(payment.CreditCard);
        }

        [Fact(DisplayName = "Should not add invalid credit card")]
        [Trait("Payment", "ManagementPayment - Payment")]
        public void AddCreditCard_ShouldNotAdd_WhenCardIsInvalid()
        {
            // Arrange
            var payment = new Payment(Guid.NewGuid(), 100);
            var invalidCard = (CreditCard)null;

            // Act
            payment.AddCreditCard(invalidCard);

            // Assert
            Assert.Null(payment.CreditCard);
        }
    }
}

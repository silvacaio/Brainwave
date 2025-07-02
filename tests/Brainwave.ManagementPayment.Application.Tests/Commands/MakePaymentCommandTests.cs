using System;
using Brainwave.ManagementPayment.Application.Commands;
using Xunit;

namespace Brainwave.ManagementPayment.Tests.Commands
{
    public class MakePaymentCommandTests
    {
        [Fact(DisplayName = "Should be valid when all fields are correctly filled")]
        [Trait("Payment", "MakePaymentCommand")]
        public void MakePaymentCommand_Should_BeValid()
        {
            var command = new MakePaymentCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "4111111111111111",
                "John Doe",
                DateTime.Today.AddMonths(1),
                "123",
                100.00m
            );

            var isValid = command.IsValid();

            Assert.True(isValid);
        }

        [Fact(DisplayName = "Should be invalid when UserId is Guid.Empty")]
        [Trait("Payment", "MakePaymentCommand")]
        public void MakePaymentCommand_Should_BeInvalid_WhenUserIdIsEmpty()
        {
            var command = new MakePaymentCommand(
                Guid.Empty,
                Guid.NewGuid(),
                "4111111111111111",
                "John Doe",
                DateTime.Today.AddMonths(1),
                "123",
                100.00m
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.PropertyName == "UserId");
        }

        [Fact(DisplayName = "Should be invalid when EnrollmentId is Guid.Empty")]
        [Trait("Payment", "MakePaymentCommand")]
        public void MakePaymentCommand_Should_BeInvalid_WhenEnrollmentIdIsEmpty()
        {
            var command = new MakePaymentCommand(
                Guid.NewGuid(),
                Guid.Empty,
                "4111111111111111",
                "John Doe",
                DateTime.Today.AddMonths(1),
                "123",
                100.00m
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.PropertyName == "EnrollmentId");
        }

        [Fact(DisplayName = "Should be invalid when CardNumber is empty")]
        [Trait("Payment", "MakePaymentCommand")]
        public void MakePaymentCommand_Should_BeInvalid_WhenCardNumberIsEmpty()
        {
            var command = new MakePaymentCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "",
                "John Doe",
                DateTime.Today.AddMonths(1),
                "123",
                100.00m
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.PropertyName == "CardNumber");
        }

        [Fact(DisplayName = "Should be invalid when CardHolderName is empty")]
        [Trait("Payment", "MakePaymentCommand")]
        public void MakePaymentCommand_Should_BeInvalid_WhenCardHolderNameIsEmpty()
        {
            var command = new MakePaymentCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "4111111111111111",
                "",
                DateTime.Today.AddMonths(1),
                "123",
                100.00m
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.PropertyName == "CardHolderName");
        }

        [Fact(DisplayName = "Should be invalid when ExpirationDate is in the past")]
        [Trait("Payment", "MakePaymentCommand")]
        public void MakePaymentCommand_Should_BeInvalid_WhenExpirationDateIsInPast()
        {
            var command = new MakePaymentCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "4111111111111111",
                "John Doe",
                DateTime.Today.AddDays(-1),
                "123",
                100.00m
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.PropertyName == "ExpirationDate");
        }

        [Fact(DisplayName = "Should be invalid when SecurityCode is empty")]
        [Trait("Payment", "MakePaymentCommand")]
        public void MakePaymentCommand_Should_BeInvalid_WhenSecurityCodeIsEmpty()
        {
            var command = new MakePaymentCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "4111111111111111",
                "John Doe",
                DateTime.Today.AddMonths(1),
                "",
                100.00m
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.PropertyName == "SecurityCode");
        }

        [Fact(DisplayName = "Should be invalid when Value is less than or equal to zero")]
        [Trait("Payment", "MakePaymentCommand")]
        public void MakePaymentCommand_Should_BeInvalid_WhenValueIsZeroOrNegative()
        {
            var command = new MakePaymentCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "4111111111111111",
                "John Doe",
                DateTime.Today.AddMonths(1),
                "123",
                0m
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.PropertyName == "Value");
        }
    }
}

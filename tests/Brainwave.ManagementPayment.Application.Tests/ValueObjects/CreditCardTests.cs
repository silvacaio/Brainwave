using System;
using Brainwave.ManagementPayment.Application.ValueObjects;
using Xunit;

namespace Brainwave.ManagementPayment.Tests.ValueObjects
{
    public class CreditCardTests
    {
        [Fact(DisplayName = "Deve criar um cartão válido")]
        [Trait("Payment", "CreditCard")]
        public void Constructor_Should_CreateValidCreditCard()
        {
            var cardNumber = "4111111111111111";
            var cardHolderName = "John Doe";
            var expirationDate = DateTime.UtcNow.Date.AddMonths(1);
            var securityCode = "123";

            var creditCard = new CreditCard(cardNumber, cardHolderName, expirationDate, securityCode);

            Assert.Equal(cardNumber, creditCard.CardNumber);
            Assert.Equal(cardHolderName, creditCard.CardHolderName);
            Assert.Equal(expirationDate, creditCard.ExpirationDate);
            Assert.Equal(securityCode, creditCard.SecurityCode);
        }

        [Theory(DisplayName = "Deve lançar ArgumentException para dados inválidos")]
        [Trait("Payment", "CreditCard")]
        [InlineData(null, "John Doe", "123")]
        [InlineData("", "John Doe", "123")]
        [InlineData("4111111111111111", null, "123")]
        [InlineData("4111111111111111", "", "123")]
        [InlineData("4111111111111111", "John Doe", null)]
        [InlineData("4111111111111111", "John Doe", "")]
        public void Constructor_ShouldThrowArgumentException_ForInvalidStrings(string cardNumber, string cardHolderName, string securityCode)
        {
            var expirationDate = DateTime.UtcNow.Date.AddMonths(1);

            Assert.Throws<ArgumentException>(() =>
                new CreditCard(cardNumber, cardHolderName, expirationDate, securityCode));
        }

        [Fact(DisplayName = "Deve lançar ArgumentException para cartão expirado")]
        [Trait("Payment", "CreditCard")]
        public void Constructor_ShouldThrowArgumentException_ForExpiredCard()
        {
            var cardNumber = "4111111111111111";
            var cardHolderName = "John Doe";
            var expirationDate = DateTime.UtcNow.Date.AddDays(-1);
            var securityCode = "123";

            Assert.Throws<ArgumentException>(() =>
                new CreditCard(cardNumber, cardHolderName, expirationDate, securityCode));
        }

        [Fact(DisplayName = "Equals deve retornar true para cartões iguais")]
        [Trait("Payment", "CreditCard")]
        public void Equals_ShouldReturnTrue_ForEqualCreditCards()
        {
            var expirationDate = DateTime.UtcNow.Date.AddMonths(1);

            var card1 = new CreditCard("4111111111111111", "John Doe", expirationDate, "123");
            var card2 = new CreditCard("4111111111111111", "John Doe", expirationDate, "123");

            Assert.True(card1.Equals(card2));
            Assert.True(card1.Equals((object)card2));
            Assert.Equal(card1.GetHashCode(), card2.GetHashCode());
        }

        [Fact(DisplayName = "Equals deve retornar false para cartões diferentes")]
        [Trait("Payment", "CreditCard")]
        public void Equals_ShouldReturnFalse_ForDifferentCreditCards()
        {
            var expirationDate = DateTime.UtcNow.Date.AddMonths(1);

            var card1 = new CreditCard("4111111111111111", "John Doe", expirationDate, "123");
            var card2 = new CreditCard("4111111111111112", "John Doe", expirationDate, "123");

            Assert.False(card1.Equals(card2));
            Assert.False(card1.Equals((object)card2));
        }

        [Fact(DisplayName = "Equals deve retornar false ao comparar com null")]
        [Trait("Payment", "CreditCard")]
        public void Equals_ShouldReturnFalse_WhenComparedWithNull()
        {
            var expirationDate = DateTime.UtcNow.Date.AddMonths(1);
            var card = new CreditCard("4111111111111111", "John Doe", expirationDate, "123");

            Assert.False(card.Equals(null));
            Assert.False(card.Equals((object)null));
        }
    }
}

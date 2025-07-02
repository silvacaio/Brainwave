namespace Brainwave.ManagementPayment.Application.ValueObjects
{
    public class CreditCard : IEquatable<CreditCard>
    {
        public string CardNumber { get; }
        public string CardHolderName { get; }
        public DateTime ExpirationDate { get; }
        public string SecurityCode { get; }

        public CreditCard(string cardNumber, string cardHolderName, DateTime expirationDate, string securityCode)
        {
            Validate(cardNumber, cardHolderName, expirationDate, securityCode);

            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            ExpirationDate = expirationDate;
            SecurityCode = securityCode;
        }

        private static void Validate(string cardNumber, string cardHolderName, DateTime expirationDate, string securityCode)
        {
            if (string.IsNullOrWhiteSpace(cardNumber)) throw new ArgumentException("Invalid card number");
            if (string.IsNullOrWhiteSpace(cardHolderName)) throw new ArgumentException("Invalid cardholder name");
            if (expirationDate < DateTime.UtcNow.Date) throw new ArgumentException("Card is expired");
            if (string.IsNullOrWhiteSpace(securityCode)) throw new ArgumentException("Invalid security code");
        }

        public override bool Equals(object obj) => Equals(obj as CreditCard);

        public bool Equals(CreditCard other)
        {
            if (other is null) return false;
            return CardNumber == other.CardNumber &&
                   CardHolderName == other.CardHolderName &&
                   ExpirationDate == other.ExpirationDate &&
                   SecurityCode == other.SecurityCode;
        }

        public override int GetHashCode() =>
            HashCode.Combine(CardNumber, CardHolderName, ExpirationDate, SecurityCode);       
    }

}

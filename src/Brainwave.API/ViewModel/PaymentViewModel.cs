﻿namespace Brainwave.API.ViewModel
{
    public class PaymentViewModel
    {
        public Guid EnrollmentId { get; set; }  
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal Value { get; set; }
    }
}

using System;

namespace RapidPayPayment
{
    public class CreditCard
    {
        public string CardNumber { get; set; }
        public string Name { get; set; }
        public short ExpirationMonth { get; set; }
        public short ExpirationYear { get; set; }
        public short VerificationCode { get; set; }
        public double Balance { get; set; }
    }
}

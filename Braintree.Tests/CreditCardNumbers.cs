namespace Braintree.Tests
{
    namespace CreditCardNumbers
    {
        public class CardTypeIndicators
        {
            public static readonly string Prepaid         = "4111111111111210";
            public static readonly string Commercial      = "4111111111131010";
            public static readonly string Payroll         = "4111111114101010";
            public static readonly string Healthcare      = "4111111510101010";
            public static readonly string DurbinRegulated = "4111161010101010";
            public static readonly string Debit           = "4117101010101010";
            public static readonly string Unknown         = "4111111111112101";
            public static readonly string No              = "4111111111310101";
        }
        public class FailsSandboxVerification
        {
            public static readonly string Amex = "378734493671000";
            public static readonly string Discover = "6011000990139424";
            public static readonly string Visa = "4000111111111115";
            public static readonly string MasterCard = "5105105105105100";
        }
      }
}

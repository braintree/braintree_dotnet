#pragma warning disable 1591

namespace Braintree
{
    public class SandboxValues
    {
        public class CreditCardNumber
        {
            public const string VISA = "4111111111111111";
            public const string MASTER_CARD = "5555555555554444";
            public const string AMEX = "371449635392376";
            public const string FRAUD = "4000111111111511";

            public class FailsVerification
            {
                public const string VISA = "4000111111111115";
            }

            public class AmexPayWithPoints
            {
                public const string SUCCESS = "371260714673002";
                public const string INELIGIBLE_CARD = "378267515471109";
                public const string INSUFFICIENT_POINTS = "371544868764018";
            }
        }

        public class Dispute
        {
            public const string CHARGEBACK = "4023898493988028";
        }

        public class TransactionAmount
        {
            public const decimal AUTHORIZE = 1000;
            public const decimal DECLINE = 2000;
            public const decimal HARD_DECLINE = 2015;
            public const decimal FAILED = 3000;
        }

        public class Nonce
        {
            public const string APPLE_PAY_VISA = "fake-apple-pay-visa-nonce";
            public const string APPLE_PAY_MASTERCARD = "fake-apple-pay-mastercard-nonce";
            public const string APPLE_PAY_AMEX = "fake-apple-pay-amex-nonce";
        }

        public class VenmoSdk
        {
            public const string VISA_PAYMENT_METHOD_CODE = "stub-4111111111111111";
            public const string INVALID_PAYMENT_METHOD_CODE = "stub-invalid-payment-method-code";
            public const string SESSION = "stub-session";
            public const string INVALID_SESSION = "stub-invalid-session";

            public static string GenerateStubPaymentMethodCode(string creditCardNumber)
            {
                return "stub-" + creditCardNumber;
            }
        }
    }
}

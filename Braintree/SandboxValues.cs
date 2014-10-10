#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class SandboxValues
    {
        public class CreditCardNumber
        {
            public const String VISA = "4111111111111111";
            public const String MASTER_CARD = "5555555555554444";
            public const String FRAUD = "4000111111111511";
        }

        public class TransactionAmount
        {
            public const Decimal AUTHORIZE = 1000;
            public const Decimal DECLINE = 2000;
            public const Decimal FAILED = 3000;
        }

        public class Nonce
        {
            public const String APPLE_PAY_VISA = "fake-apple-pay-visa-nonce";
            public const String APPLE_PAY_MASTERCARD = "fake-apple-pay-mastercard-nonce";
            public const String APPLE_PAY_AMEX = "fake-apple-pay-amex-nonce";
        }

        public class VenmoSdk
        {
            public const String VISA_PAYMENT_METHOD_CODE = "stub-4111111111111111";
            public const String INVALID_PAYMENT_METHOD_CODE = "stub-invalid-payment-method-code";
            public const String SESSION = "stub-session";
            public const String INVALID_SESSION = "stub-invalid-session";

            public static String GenerateStubPaymentMethodCode(String creditCardNumber)
            {
                return "stub-" + creditCardNumber;
            }
        }
    }
}

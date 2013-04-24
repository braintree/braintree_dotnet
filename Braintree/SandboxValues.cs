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
        }

        public class TransactionAmount
        {
            public const Decimal AUTHORIZE = 1000;
            public const Decimal DECLINE = 2000;
            public const Decimal FAILED = 3000;
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

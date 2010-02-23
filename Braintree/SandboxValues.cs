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
        }
    }
}

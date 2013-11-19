using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class AuthorizationFingerprintOptions {
        public String CustomerId { get; set; }
        public Boolean MakeDefault { get; set; }
        public Boolean FailOnDuplicatePaymentMethod { get; set; }
        public Boolean VerifyCard { get; set; }
    }
}

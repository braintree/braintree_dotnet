using System;
using System.Collections.Generic;

namespace Braintree
{
    public class ThreeDSecureLookup
    {
        public virtual string AcsUrl { get; protected set; }
        public virtual string ThreeDSecureVersion { get; protected set; }
        public virtual string TransactionId { get; protected set; }

        public ThreeDSecureLookup(dynamic lookupResponse)
        {
            AcsUrl = lookupResponse.acsUrl;
            ThreeDSecureVersion = lookupResponse.threeDSecureVersion;
            TransactionId = lookupResponse.transactionId;
        }

        [Obsolete("Mock Use Only")]
        protected internal ThreeDSecureLookup() { }
    }
}


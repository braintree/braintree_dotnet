using System;

namespace Braintree
{
    public class RecipientPhoneDetails
    {
        public virtual string CountryCode { get; protected set; }
        public virtual string NationalNumber { get; protected set; }

        protected internal RecipientPhoneDetails(NodeWrapper node)
        {
            CountryCode = node.GetString("country-code"); 
            NationalNumber = node.GetString("national-number");
        }
         
    }
}

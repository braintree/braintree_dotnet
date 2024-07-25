using System;
using System.ComponentModel;

namespace Braintree
{
    public class InternationalPhone 
    {
        public virtual string CountryCode { get; protected set; }
        public virtual string NationalNumber { get; protected set; }

        protected internal InternationalPhone(NodeWrapper node) 
        {
            CountryCode = node.GetString("country-code");
            NationalNumber = node.GetString("national-number");
        }

        [Obsolete("Mock Use Only")]
        protected internal InternationalPhone() { }
    }
}

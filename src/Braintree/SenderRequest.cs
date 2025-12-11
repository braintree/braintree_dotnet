#pragma warning disable 1591

using System.Collections.Generic;
using System;

namespace Braintree 
{
    public class SenderRequest: Request 
    {
        public string AccountReferenceNumber { get; set; }
        public AddressRequest Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string TaxId { get; set; }

        public override string ToXml()
        {
            return ToXml("sender");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("sender");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);
            
            if (FirstName != null) {
                builder.AddElement("first-name", FirstName);
            }
            
            if (LastName != null) {
                builder.AddElement("last-name", LastName);
            }

            if (MiddleName != null) {
                builder.AddElement("middle-name", MiddleName);
            }
            
            if (AccountReferenceNumber != null) {
                builder.AddElement("account-reference-number", AccountReferenceNumber);
            }
            
            if (TaxId != null) {
                builder.AddElement("tax-id", TaxId);
            }

            if (DateOfBirth != null) {
                builder.AddElement("date-of-birth", DateOfBirth);
            }
            
            if (Address != null) {
                builder.AddElement("address", Address);
            }
            
            return builder;
        }
    }
}

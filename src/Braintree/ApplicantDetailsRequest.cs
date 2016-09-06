#pragma warning disable 1591

namespace Braintree
{
    public class ApplicantDetailsRequest : Request
    {
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AddressRequest Address { get; set; }
        public string DateOfBirth { get; set; }
        public string Ssn { get; set; }
        public string TaxId { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }

        public override string ToXml()
        {
            return ToXml("applicant-details");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("applicant-details");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("company-name", CompanyName).
                AddElement("first-name", FirstName).
                AddElement("last-name", LastName).
                AddElement("email", Email).
                AddElement("phone", Phone).
                AddElement("address", Address).
                AddElement("date-of-birth", DateOfBirth).
                AddElement("ssn", Ssn).
                AddElement("tax-id", TaxId).
                AddElement("routing-number", RoutingNumber).
                AddElement("account-number", AccountNumber);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class CustomerRequest : Request
    {
        public String Id { get; set; }
        public String CustomerId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Company { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String Fax { get; set; }
        public String Website { get; set; }
        public Dictionary<String, String> CustomFields { get; set; }
        public CreditCardRequest CreditCard { get; set; }

        internal override String ToXml()
        {
            return ToXml("customer");
        }

        internal override String ToXml(String rootElement)
        {
            var builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));
            builder.Append(BuildXMLElement("id", Id));
            builder.Append(BuildXMLElement("first-name", FirstName));
            builder.Append(BuildXMLElement("last-name", LastName));
            builder.Append(BuildXMLElement("company", Company));
            builder.Append(BuildXMLElement("email", Email));
            builder.Append(BuildXMLElement("phone", Phone));
            builder.Append(BuildXMLElement("fax", Fax));
            builder.Append(BuildXMLElement("website", Website));
            builder.Append(BuildXMLElement(CreditCard));
            builder.Append(BuildXMLElement("custom-fields", CustomFields));
            builder.Append(String.Format("</{0}>", rootElement));

            return builder.ToString();
        }

        public override String ToQueryString()
        {
            return ToQueryString("customer");
        }

        public override String ToQueryString(String root)
        {
            return new QueryString().
                Append(ParentBracketChildString(root, "id"), Id).
                Append(ParentBracketChildString(root, "first_name"), FirstName).
                Append(ParentBracketChildString(root, "last_name"), LastName).
                Append(ParentBracketChildString(root, "company"), Company).
                Append(ParentBracketChildString(root, "email"), Email).
                Append(ParentBracketChildString(root, "phone"), Phone).
                Append(ParentBracketChildString(root, "fax"), Fax).
                Append(ParentBracketChildString(root, "website"), Website).
                Append(ParentBracketChildString(root, "credit_card"), CreditCard).
                Append(ParentBracketChildString(root, "custom_fields"), CustomFields).
                Append("customer_id", CustomerId).
                ToString();
        }
    }
}


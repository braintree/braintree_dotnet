#pragma warning disable 1591

using System;

namespace Braintree
{
    public class MerchantAccountIndividualDetails
    {
        public String FirstName { get; protected set; }
        public String LastName { get; protected set; }
        public String Email { get; protected set; }
        public String Phone { get; protected set; }
        public String DateOfBirth { get; protected set; }
        public Address Address { get; protected set; }

        protected internal MerchantAccountIndividualDetails(NodeWrapper node)
        {
            FirstName = node.GetString("first-name");
            LastName = node.GetString("last-name");
            Email = node.GetString("email");
            Phone = node.GetString("phone");
            DateOfBirth = node.GetString("date-of-birth");
            Address = new Address(node.GetNode("address"));
        }
    }
}

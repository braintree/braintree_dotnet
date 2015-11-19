#pragma warning disable 1591

using System;

namespace Braintree
{
    public class MerchantAccountIndividualDetails
    {
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string Phone { get; protected set; }
        public virtual string DateOfBirth { get; protected set; }
        public virtual string SsnLastFour { get; protected set; }
        public virtual Address Address { get; protected set; }

        protected internal MerchantAccountIndividualDetails(NodeWrapper node)
        {
            FirstName = node.GetString("first-name");
            LastName = node.GetString("last-name");
            Email = node.GetString("email");
            Phone = node.GetString("phone");
            DateOfBirth = node.GetString("date-of-birth");
            SsnLastFour = node.GetString("ssn-last-4");
            Address = new Address(node.GetNode("address"));
        }

        [Obsolete("Mock Use Only")]
        protected internal MerchantAccountIndividualDetails() { }
    }
}

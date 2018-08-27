#pragma warning disable 1591

using System;

namespace Braintree
{
    public class CustomerDetails
    {
        public virtual string Id { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string Company { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string Phone { get; protected set; }
        public virtual string Fax { get; protected set; }
        public virtual string Website { get; protected set; }

        public CustomerDetails(NodeWrapper node, IBraintreeGateway gateway)
        {
            if (node == null) return;

            Id = node.GetString("id");
            FirstName = node.GetString("first-name");
            LastName = node.GetString("last-name");
            Company = node.GetString("company");
            Email = node.GetString("email");
            Phone = node.GetString("phone");
            Fax = node.GetString("fax");
            Website = node.GetString("website");
        }

        protected internal CustomerDetails() { }
    }
}

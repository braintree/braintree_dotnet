using System;

namespace Braintree
{
    public class VenmoProfileData
    {
        public virtual Address BillingAddress { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string PhoneNumber { get; protected set; }
        public virtual Address ShippingAddress { get; protected set; }
        public virtual string Username { get; protected set; }

        protected internal VenmoProfileData(NodeWrapper node) {
            Username = node.GetString("username");
            FirstName = node.GetString("first-name");
            LastName = node.GetString("last-name");
            PhoneNumber = node.GetString("phone-number");
            Email = node.GetString("email");

            var billingAddressNode = node.GetNode("billing-address");
            if (billingAddressNode != null) {
                BillingAddress = new Address(billingAddressNode);
            }

            var shippingAddressNode = node.GetNode("shipping-address");
            if (shippingAddressNode != null) {
                ShippingAddress = new Address(shippingAddressNode);
            }
        }
    }
}

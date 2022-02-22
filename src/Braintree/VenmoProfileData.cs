using System;

namespace Braintree
{
    public class VenmoProfileData
    {
        public virtual string Username { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string PhoneNumber { get; protected set; }
        public virtual string Email { get; protected set; }

        protected internal VenmoProfileData(NodeWrapper node) {
            Username = node.GetString("username");
            FirstName = node.GetString("first-name");
            LastName = node.GetString("last-name");
            PhoneNumber = node.GetString("phone-number");
            Email = node.GetString("email");
        }
    }
}

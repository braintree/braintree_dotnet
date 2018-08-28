using System;

namespace Braintree
{
    public class UsBankAccount : PaymentMethod
    {
        public virtual string RoutingNumber { get; protected set; }
        public virtual string Last4 { get; protected set; }
        public virtual string AccountType { get; protected set; }
        public virtual string AccountHolderName { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string BankName { get; protected set; }
        public virtual string CustomerId { get; protected set; }
        public virtual AchMandate AchMandate { get; protected set; }
        public virtual bool? IsDefault { get; protected set; }

        protected internal UsBankAccount(NodeWrapper node)
        {
            RoutingNumber = node.GetString("routing-number");
            Last4 = node.GetString("last-4");
            AccountType = node.GetString("account-type");
            AccountHolderName = node.GetString("account-holder-name");
            Token = node.GetString("token");
            ImageUrl = node.GetString("image-url");
            BankName = node.GetString("bank-name");
            CustomerId = node.GetString("customer-id");
            AchMandate = new AchMandate(node.GetNode("ach-mandate"));
            IsDefault = node.GetBoolean("default");
        }

        [Obsolete("Mock Use Only")]
        protected internal UsBankAccount() { }
    }
}

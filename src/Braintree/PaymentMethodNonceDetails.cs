using System;

namespace Braintree
{
    public class PaymentMethodNonceDetails
    {
        public virtual string Bin { get; protected set; }
        public virtual string LastTwo { get; protected set; }
        public virtual string LastFour { get; protected set; }
        public virtual string CardType { get; protected set; }
        public virtual string ExpirationYear { get; protected set; }
        public virtual string ExpirationMonth { get; protected set; }
        public virtual string Username { get; protected set; }
        public virtual string VenmoUserId { get; protected set; }
        public virtual PaymentMethodNonceDetailsPayerInfo PayerInfo { get; protected set; }

        protected internal PaymentMethodNonceDetails(NodeWrapper node)
        {
            Bin = node.GetString("bin");
            CardType = node.GetString("card-type");
            LastTwo = node.GetString("last-two");
            LastFour = node.GetString("last-four");
            ExpirationYear = node.GetString("expiration-year");
            ExpirationMonth = node.GetString("expiration-month");
            Username = node.GetString("username");
            VenmoUserId = node.GetString("venmo-user-id");
            var payerInfoNode = node.GetNode("payer-info");
            if (payerInfoNode != null)
            {
                PayerInfo = new PaymentMethodNonceDetailsPayerInfo(payerInfoNode);
            }
        }

        protected internal PaymentMethodNonceDetails(dynamic details)
        {
            Bin = details.bin;
            CardType = details.cardType;
            LastTwo = details.lastTwo;
            LastFour = details.lastFour;
            ExpirationYear = details.expirationYear;
            ExpirationMonth = details.expirationMonth;
            Username = details.username;
            VenmoUserId = details.venmoUserId;

            var payerInfo = details.payerInfo;
            if (payerInfo != null)
            {
                PayerInfo = new PaymentMethodNonceDetailsPayerInfo(payerInfo);
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal PaymentMethodNonceDetails() { }
    }
}

using System;

namespace Braintree
{
    public class PaymentMethodNonceDetails
    {
        public virtual PaymentMethodNonceDetailsPayerInfo PayerInfo { get; protected set; }
        public virtual PaymentMethodNonceDetailsSepaDirectDebit SepaDirectDebit { get; protected set; }
        public virtual bool? IsNetworkTokenized { get; protected set; }
        public virtual string Bin { get; protected set; }
        //NEXT_MAJOR_VERSION CardType should be an enum (see CreditCard class)
        public virtual string CardType { get; protected set; }
        public virtual string ExpirationMonth { get; protected set; }
        public virtual string ExpirationYear { get; protected set; }
        public virtual string LastFour { get; protected set; }
        public virtual string LastTwo { get; protected set; }
        public virtual string Username { get; protected set; }
        public virtual string VenmoUserId { get; protected set; }

        protected internal PaymentMethodNonceDetails(NodeWrapper node)
        {
            Bin = node.GetString("bin");
            CardType = node.GetString("card-type");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            IsNetworkTokenized = node.GetBoolean("is-network-tokenized");
            LastFour = node.GetString("last-four");
            LastTwo = node.GetString("last-two");
            Username = node.GetString("username");
            VenmoUserId = node.GetString("venmo-user-id");
            
            var payerInfoNode = node.GetNode("payer-info");
            if (payerInfoNode != null)
            {
                PayerInfo = new PaymentMethodNonceDetailsPayerInfo(payerInfoNode);
            }

            if (node.GetNode("bank-reference-token") != null && node.GetNode("iban-last-chars") != null)
            {
                SepaDirectDebit = new PaymentMethodNonceDetailsSepaDirectDebit(node);

            }
        }

        protected internal PaymentMethodNonceDetails(dynamic details)
        {
            Bin = details.bin;
            CardType = details.cardType;
            ExpirationMonth = details.expirationMonth;
            ExpirationYear = details.expirationYear;
            IsNetworkTokenized = details.isNetworkTokenized;
            LastFour = details.lastFour;
            LastTwo = details.lastTwo;
            Username = details.username;
            VenmoUserId = details.venmoUserId;

            var payerInfo = details.payerInfo;
            if (payerInfo != null)
            {
                PayerInfo = new PaymentMethodNonceDetailsPayerInfo(payerInfo);
            }

            if (details.bankReferenceToken != null && details.ibanLastChars != null)
            {
                SepaDirectDebit = new PaymentMethodNonceDetailsSepaDirectDebit(details);
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal PaymentMethodNonceDetails() { }
    }
}

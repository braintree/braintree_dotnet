using System;

namespace Braintree
{
    public class AndroidPayDetails
    {
        public String Bin { get; protected set; }
        public String ExpirationMonth { get; protected set; }
        public String ExpirationYear { get; protected set; }
        public String GoogleTransactionId { get; protected set; }
        public String ImageUrl { get; protected set; }
        public String SourceCardLast4 { get; protected set; }
        public String SourceCardType { get; protected set; }
        public String VirtualCardLast4 { get; protected set; }
        public String VirtualCardType { get; protected set; }
        public String CardType { get; protected set; }
        public String Last4 { get; protected set; }
        public String Token { get; protected set; }

        protected internal AndroidPayDetails(NodeWrapper node)
        {
            Bin = node.GetString("bin");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            GoogleTransactionId = node.GetString("google-transaction-id");
            ImageUrl = node.GetString("image-url");
            SourceCardType = node.GetString("source-card-type");
            SourceCardLast4 = node.GetString("source-card-last-4");
            VirtualCardLast4 = node.GetString("virtual-card-last-4");
            VirtualCardType = node.GetString("virtual-card-type");
            CardType = node.GetString("virtual-card-type");
            Last4 = node.GetString("virtual-card-last-4");
            Token = node.GetString("token");
        }
    }
}

using System;

namespace Braintree
{
    public class AndroidPayDetails
    {
        public string Bin { get; protected set; }
        public string ExpirationMonth { get; protected set; }
        public string ExpirationYear { get; protected set; }
        public string GoogleTransactionId { get; protected set; }
        public string ImageUrl { get; protected set; }
        public string SourceCardLast4 { get; protected set; }
        public string SourceCardType { get; protected set; }
        public string SourceDescription { get; protected set; }
        public string VirtualCardLast4 { get; protected set; }
        public string VirtualCardType { get; protected set; }
        public string CardType { get; protected set; }
        public string Last4 { get; protected set; }
        public string Token { get; protected set; }

        protected internal AndroidPayDetails(NodeWrapper node)
        {
            Bin = node.GetString("bin");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            GoogleTransactionId = node.GetString("google-transaction-id");
            ImageUrl = node.GetString("image-url");
            SourceCardType = node.GetString("source-card-type");
            SourceCardLast4 = node.GetString("source-card-last-4");
            SourceDescription = node.GetString("source-description");
            VirtualCardLast4 = node.GetString("virtual-card-last-4");
            VirtualCardType = node.GetString("virtual-card-type");
            CardType = node.GetString("virtual-card-type");
            Last4 = node.GetString("virtual-card-last-4");
            Token = node.GetString("token");
        }
    }
}

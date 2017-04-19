namespace Braintree
{
    public class IdealPaymentDetails
    {
        public virtual string IdealPaymentId { get; protected set; }
        public virtual string IdealTransactionId { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string MaskedIban { get; protected set; }
        public virtual string Bic { get; protected set; }

        protected internal IdealPaymentDetails(NodeWrapper node)
        {
            IdealPaymentId = node.GetString("ideal-payment-id");
            IdealTransactionId = node.GetString("ideal-transaction-id");
            ImageUrl = node.GetString("image-url");
            MaskedIban = node.GetString("masked-iban");
            Bic = node.GetString("bic");
        }
    }
}

using System;
using System.Collections.Generic;

namespace Braintree
{
    public class LocalPaymentCompleted
    {
        public virtual string Bic { get; protected set; }
        public virtual List<BlikAlias> BlikAliases { get; protected set; }
        public virtual string IbanLastChars { get; protected set; }
        public virtual string PayerId { get; protected set; }
        public virtual string PayerName { get; protected set; }
        public virtual string PaymentId { get; protected set; }
        public virtual string PaymentMethodNonce { get; protected set; }
        public virtual Transaction Transaction { get; protected set; }

        protected internal LocalPaymentCompleted(NodeWrapper node, IBraintreeGateway gateway)
        {
            Bic = node.GetString("bic");
            IbanLastChars = node.GetString("iban-last-chars");
            PayerId = node.GetString("payer-id");
            PayerName = node.GetString("payer-name");
            PaymentId = node.GetString("payment-id");
            PaymentMethodNonce = node.GetString("payment-method-nonce");

            var transactionNode = node.GetNode("transaction");
            if(transactionNode != null)
            {
                Transaction = new Transaction(transactionNode, gateway);
            }

            BlikAliases = new List<BlikAlias>();
            foreach (var blikAliasNode in node.GetList("blik-aliases/blik-alias")) {
                BlikAliases.Add(new BlikAlias(blikAliasNode));
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal LocalPaymentCompleted() { }
    }
}

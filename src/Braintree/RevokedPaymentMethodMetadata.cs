using System;
using System.Collections.Generic;
using Braintree.Exceptions;

namespace Braintree
{
    public class RevokedPaymentMethodMetadata
    {
        public virtual string CustomerId { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual PaymentMethod RevokedPaymentMethod { get; protected set; }

        protected internal RevokedPaymentMethodMetadata(NodeWrapper node, IBraintreeGateway gateway) {
            if (node.GetChildren().Count == 0) {
                throw new UnexpectedException();
            }

            RevokedPaymentMethod = PaymentMethodParser.ParsePaymentMethod(node.GetChildren()[0], gateway);
            CustomerId = RevokedPaymentMethod.CustomerId;
            Token = RevokedPaymentMethod.Token;
        }
    }
}

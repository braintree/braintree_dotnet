using System;
using Braintree.Exceptions;

namespace Braintree
{
    public class PaymentMethodCustomerDataUpdatedMetadata
    {
        public virtual string Token { get; protected set; }
        public virtual string DateTimeUpdated { get; protected set; }
        public virtual EnrichedCustomerData EnrichedCustomerData { get; protected set; }
        public virtual PaymentMethod CustomerPaymentMethod { get; protected set; }

        protected internal PaymentMethodCustomerDataUpdatedMetadata(NodeWrapper node, IBraintreeGateway gateway) {
            if (node.GetChildren().Count == 0) {
                throw new UnexpectedException();
            }

            CustomerPaymentMethod = PaymentMethodParser.ParsePaymentMethod(node.GetNode("payment-method"), gateway);
            Token = node.GetString("token");
            DateTimeUpdated = node.GetString("datetime-updated");

            var customerDataNode = node.GetNode("enriched-customer-data");
            if(customerDataNode != null)
            {
                EnrichedCustomerData = new EnrichedCustomerData(customerDataNode);
            }
        }
    }
}

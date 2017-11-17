using System;
using System.Collections.Generic;

namespace Braintree
{
    public class GrantedPaymentInstrumentUpdate
    {
        public virtual string PaymentMethodNonce { get; protected set; }
        public virtual string GrantOwnerMerchantId { get; protected set; }
        public virtual string GrantRecipientMerchantId { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual List<string> UpdatedFields { get; protected set; }

        protected internal GrantedPaymentInstrumentUpdate(NodeWrapper node)
        {
            PaymentMethodNonce = node.GetString("payment-method-nonce/nonce");
            GrantOwnerMerchantId = node.GetString("grant-owner-merchant-id");
            GrantRecipientMerchantId = node.GetString("grant-recipient-merchant-id");
            Token = node.GetString("token");

            UpdatedFields = new List<string>();
            foreach (var field in node.GetList("updated-fields/item"))
            {
                UpdatedFields.Add(field.GetString("."));
            }
        }
    }
}

#pragma warning disable 1591

using Braintree.Exceptions;
using System.Collections.Generic;

namespace Braintree
{
    /// <summary>
    /// A class for building requests to manipulate <see cref="PaymentMethodNonce"/> records in the vault.
    /// </summary>
    public class PaymentMethodNonceRequest : Request
    {
        public string MerchantAccountId { get; set; }
        public bool? AuthenticationInsight { get; set; }
        public AuthenticationInsightOptionsRequest AuthenticationInsightOptions { get; set; }

        public override string ToXml()
        {
            return ToXml("payment-method-nonce");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("payment-method-nonce");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);

            if (AuthenticationInsight == true) {
                if (MerchantAccountId == null)
                {
                    string errorMessage = "Merchant Account Id is required to request Authentication Insight";
                    throw new PaymentMethodNonceRequestInvalidException(errorMessage);
                }

                builder.AddElement("merchant-account-id", MerchantAccountId);
                builder.AddElement("authentication-insight", AuthenticationInsight);
                builder.AddElement("authentication-insight-options", AuthenticationInsightOptions);
            }

            return builder;
        }
    }
}

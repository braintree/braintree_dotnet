#pragma warning disable 1591

using System.Collections.Generic;

namespace Braintree
{
    /// <summary>
    /// Provides a fluent interface to build requests for creating Bank Account Instant Verification JWTs.
    /// </summary>
    public class BankAccountInstantVerificationJwtRequest : Request
    {
        /// <summary>
        /// Sets the officially registered business name for the merchant.
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// Sets the URL to redirect the consumer after successful account selection.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Sets the URL to redirect the consumer upon cancellation of the Open Banking flow.
        /// </summary>
        public string CancelUrl { get; set; }


        public override string ToXml()
        {
            return ToXml("bankAccountInstantVerificationJwtRequest");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return BuildQueryStringRequest().ToQueryString();
        }

        public override string ToQueryString(string root)
        {
            return BuildQueryStringRequest().ToQueryString();
        }

        protected virtual RequestBuilder BuildQueryStringRequest()
        {
            var builder = new RequestBuilder("");
            
            if (!string.IsNullOrEmpty(BusinessName))
                builder.AddTopLevelElement("business-name", BusinessName);
            if (!string.IsNullOrEmpty(ReturnUrl))
                builder.AddTopLevelElement("return-url", ReturnUrl);
            if (!string.IsNullOrEmpty(CancelUrl))
                builder.AddTopLevelElement("cancel-url", CancelUrl);
                
            return builder;
        }

        public virtual Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>();
            var input = new Dictionary<string, object>();

            if (BusinessName != null)
            {
                input["businessName"] = BusinessName;
            }
            if (ReturnUrl != null)
            {
                input["returnUrl"] = ReturnUrl;
            }
            if (CancelUrl != null)
            {
                input["cancelUrl"] = CancelUrl;
            }

            variables["input"] = input;
            return variables;
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root)
                .AddElement("businessName", BusinessName)
                .AddElement("returnUrl", ReturnUrl)
                .AddElement("cancelUrl", CancelUrl)
;
        }

    }
}
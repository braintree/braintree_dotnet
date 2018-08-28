#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Generates client tokens, which are used to authenticate requests clients make directly
    ///   on behalf of merchants
    /// </summary>
    public class ClientTokenGateway : IClientTokenGateway
    {
        private readonly BraintreeService Service;

        protected internal ClientTokenGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Service = new BraintreeService(gateway.Configuration);
        }

        public virtual string Generate(ClientTokenRequest request = null)
        {
            if (request == null) request = new ClientTokenRequest();
            VerifyOptions(request);
            XmlNode response = Service.Post(Service.MerchantPath() + "/client_token", request);

            if (response.Name.Equals("client-token"))
            {
                return Regex.Unescape(response.InnerText);
            }
            else
            {
                throw new ArgumentException(response.SelectSingleNode("message").InnerText);
            }
        }

        [Obsolete("Use Generate")]
        public virtual string generate(ClientTokenRequest request = null)
        {
            return Generate(request);
        }

        public virtual async Task<string> GenerateAsync(ClientTokenRequest request = null)
        {
            if (request == null) request = new ClientTokenRequest();
            VerifyOptions(request);
            XmlNode response = await Service.PostAsync(Service.MerchantPath() + "/client_token", request).ConfigureAwait(false);

            if (response.Name.Equals("client-token"))
            {
                return Regex.Unescape(response.InnerText);
            }
            else
            {
                throw new ArgumentException(response.SelectSingleNode("message").InnerText);
            }
        }

        [Obsolete("Use GenerateAsync")]
        public virtual Task<string> generateAsync(ClientTokenRequest request = null)
        {
            return GenerateAsync(request);
        }

        private void VerifyOptions(ClientTokenRequest request)
        {
            if (request.Options != null && request.CustomerId == null) {
                var invalidOptions = new List<string>{};

                if (request.Options.VerifyCard != null)
                {
                    invalidOptions.Add("VerifyCard");
                }
                if (request.Options.MakeDefault != null)
                {
                    invalidOptions.Add("MakeDefault");
                }
                if (request.Options.FailOnDuplicatePaymentMethod != null)
                {
                    invalidOptions.Add("FailOnDuplicatePaymentMethod");
                }

                if (invalidOptions.Count != 0)
                {
                    var message = "Following arguments are invalid without customerId: ";
                    foreach (var invalidOption in invalidOptions)
                    {
                        message += " " + invalidOption;
                    }
                    throw new ArgumentException(message);
                }
            }
        }
    }
}

#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using Braintree.Exceptions;

namespace Braintree
{
    /// <summary>
    /// Generates client tokens, which are used to authenticate requests clients make directly
    ///   on behalf of merchants
    /// </summary>
    public class ClientTokenGateway
    {
        private BraintreeService Service;

        protected internal ClientTokenGateway(BraintreeService service)
        {
            Service = service;
        }

        public virtual string generate(ClientTokenRequest request = null)
        {
            if (request == null) request = new ClientTokenRequest();
            verifyOptions(request);
            XmlNode response = Service.Post(Service.MerchantPath() + "/client_token", request);

            if (response.Name.Equals("client-token")) {
                return Regex.Unescape(response.InnerText);
            } else {
                throw new ArgumentException(response.SelectSingleNode("message").InnerText);
            }
        }

        private void verifyOptions(ClientTokenRequest request)
        {
            if (request.Options != null && request.CustomerId == null) {
                List<String> invalidOptions = new List<String>{};

                if (request.Options.VerifyCard != null) {
                    invalidOptions.Add("VerifyCard");
                }
                if (request.Options.MakeDefault != null) {
                    invalidOptions.Add("MakeDefault");
                }
                if (request.Options.FailOnDuplicatePaymentMethod != null) {
                    invalidOptions.Add("FailOnDuplicatePaymentMethod");
                }

                if (invalidOptions.Count != 0) {
                    string message = "Following arguments are invalid without customerId: ";
                    foreach(string invalidOption in invalidOptions) {
                        message += " " + invalidOption;
                    }
                    throw new ArgumentException(message);
                }
            }
        }
    }
}

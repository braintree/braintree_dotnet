using System;
using System.IO;
using System.Net;
#if netcore
using System.Net.Http;
#else
using System.Web;
#endif
using System.Reflection;
using System.Text;
using System.Xml;

namespace Braintree
{
    public class ThreeDSecureGateway : IThreeDSecureGateway
    {
        protected static readonly Encoding encoding = Encoding.UTF8;
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        public ThreeDSecureGateway(BraintreeGateway gateway)
        {
            this.gateway = gateway;
            service = new BraintreeService(gateway.Configuration);
        }

        public ThreeDSecureLookupResponse Lookup(ThreeDSecureLookupRequest lookupRequest)
        {
            var config = gateway.Configuration;
            var gwURL = config.Environment.GatewayURL;
            var nonce = lookupRequest.Nonce;
            var lookupURL = gwURL + "/merchants/" + config.MerchantId + "/client_api/v1/payment_methods/" + nonce + "/three_d_secure/lookup";
            var request = service.GetHttpRequest(lookupURL, "POST");
            var bodyBytes = encoding.GetBytes(lookupRequest.ToJSON());
#if netcore
            request.Headers.Add("Accept", "application/json");

            var utf8_string = encoding.GetString(bodyBytes);
            request.Content = new StringContent(utf8_string, encoding, "application/json");
            request.Content.Headers.ContentLength = System.Text.UTF8Encoding.UTF8.GetByteCount(utf8_string);
#else
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.ContentLength = bodyBytes.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bodyBytes, 0, bodyBytes.Length);
            }
#endif

            var response = service.GetHttpResponse(request);

            return new ThreeDSecureLookupResponse(response);
        }
    }
}

#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class WebhookTestingGateway
    {
        private BraintreeService Service;

        protected internal WebhookTestingGateway(BraintreeService service)
        {
            Service = service;
        }

        public virtual Dictionary<string, string> SampleNotification(WebhookKind kind, string id)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            string payload = BuildPayload(kind, id);
            response["payload"] = payload;
            response["signature"] = BuildSignature(payload);
            return response;
        }

        private string BuildPayload(WebhookKind kind, string id)
        {
            string payload = String.Format("<notification><timestamp></timestamp><kind>{0}</kind><subject>{1}</subject></notification>", kind, SubscriptionXml(id));
            return Convert.ToBase64String(Encoding.Default.GetBytes(payload)).Trim();
        }

        private string BuildSignature(string payload)
        {
            return String.Format("{0}|{1}", Service.PublicKey, new Crypto().HmacHash(Service.PrivateKey, payload).Trim().ToLower());
        }

        private String SubscriptionXml(String id)
        {
            return String.Format("<subscription><id>{0}</id><transactions type=\"array\"></transactions><add_ons type=\"array\"></add_ons><discounts type=\"array\"></discounts></subscription>", id);
        }
    }
}
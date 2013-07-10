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
            string currentTime = DateTime.Now.ToUniversalTime().ToString("u");
            string payload = String.Format("<notification><timestamp type=\"datetime\">{0}</timestamp><kind>{1}</kind><subject>{2}</subject></notification>", currentTime, kind, SubjectSampleXml(kind, id));
            return Convert.ToBase64String(Encoding.Default.GetBytes(payload)).Trim();
        }

        private string BuildSignature(string payload)
        {
            return String.Format("{0}|{1}", Service.PublicKey, new Crypto().HmacHash(Service.PrivateKey, payload).Trim().ToLower());
        }

        private String SubjectSampleXml(WebhookKind kind, String id)
        {
            if (kind == WebhookKind.SUB_MERCHANT_ACCOUNT_APPROVED) {
                return MerchantAccountApprovedSampleXml(id);
            } else if (kind == WebhookKind.SUB_MERCHANT_ACCOUNT_DECLINED) {
                return MerchantAccountDeclinedSampleXml(id);
            } else if (kind == WebhookKind.TRANSACTION_DISBURSED) {
                return TransactionDisbursedSampleXml(id);
            } else {
                return SubscriptionXml(id);
            }
        }

        private String MerchantAccountDeclinedSampleXml(String id)
        {
            return String.Format("<api-error-response><message>Applicant declined due to OFAC.</message><merchant-account><id>{0}</id><master-merchant-account><id>master_ma_for_{0}</id><status>suspended</status></master-merchant-account><status>suspended</status></merchant-account><errors><merchant-account><errors type=\"array\"><error><message>Applicant declined due to OFAC.</message><code>82621</code><attribute>base</attribute></error></errors></merchant-account></errors></api-error-response>", id);
        }

        private String TransactionDisbursedSampleXml(String id)
        {
            return String.Format("<transaction><id>{0}</id><amount>100</amount><disbursement-details><disbursement-date type=\"datetime\">2013-07-09T18:23:29Z</disbursement-date></disbursement-details><billing></billing><credit-card></credit-card><customer></customer><descriptor></descriptor><shipping></shipping><subscription></subscription></transaction>", id);
        }

        private String SubscriptionXml(String id)
        {
            return String.Format("<subscription><id>{0}</id><transactions type=\"array\"></transactions><add-ons type=\"array\"></add-ons><discounts type=\"array\"></discounts></subscription>", id);
        }

        private String MerchantAccountApprovedSampleXml(String id)
        {
            return String.Format("<merchant-account><id>{0}</id><master-merchant-account><id>master_ma_for_{0}</id><status>active</status></master-merchant-account><status>active</status></merchant-account>", id);
        }
    }
}

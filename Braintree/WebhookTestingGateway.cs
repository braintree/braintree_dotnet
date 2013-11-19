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
            return String.Format("{0}|{1}", Service.PublicKey, new Sha1Hasher().HmacHash(Service.PrivateKey, payload).Trim().ToLower());
        }

        private String SubjectSampleXml(WebhookKind kind, String id)
        {
            if (kind == WebhookKind.SUB_MERCHANT_ACCOUNT_APPROVED) {
                return MerchantAccountApprovedSampleXml(id);
            } else if (kind == WebhookKind.SUB_MERCHANT_ACCOUNT_DECLINED) {
                return MerchantAccountDeclinedSampleXml(id);
            } else if (kind == WebhookKind.TRANSACTION_DISBURSED) {
                return TransactionDisbursedSampleXml(id);
            } else if (kind == WebhookKind.PARTNER_MERCHANT_CONNECTED) {
                return PartnerMerchantConnectedSampleXml(id);
            } else if (kind == WebhookKind.PARTNER_MERCHANT_DISCONNECTED) {
                return PartnerMerchantDisconnectedSampleXml(id);
            } else if (kind == WebhookKind.PARTNER_MERCHANT_DECLINED) {
                return PartnerMerchantDeclinedSampleXml(id);
            } else {
                return SubscriptionXml(id);
            }
        }

        private static readonly string TYPE_DATETIME = "type=\"datetime\"";
        private static readonly string TYPE_ARRAY = "type=\"array\"";
        private static readonly string TYPE_SYMBOL = "type=\"symbol\"";

        private String MerchantAccountDeclinedSampleXml(String id)
        {
            return node("api-error-response",
                node("message", "Applicant declined due to OFAC."),
                node_attr("errors", TYPE_ARRAY,
                    node("merchant-account",
                        node_attr("errors", TYPE_ARRAY,
                            node("error",
                                node("code", "82621"),
                                node("message", "Applicant declined due to OFAC."),
                                node_attr("attribute", TYPE_SYMBOL, "base")
                            )
                        )
                    )
                ),
                node("merchant-account",
                    node("id", id),
                    node("status", "suspended"),
                    node("master-merchant-account",
                        node("id", "master_ma_for_" + id),
                        node("status", "suspended")
                    )
                )
            );
        }

        private String TransactionDisbursedSampleXml(String id)
        {
            return node("transaction",
                    node("id", id),
                    node("amount", "100"),
                    node("disbursement-details",
                        node_attr("disbursement-date", TYPE_DATETIME, "2013-07-09T18:23:29Z")
                    ),
                    node("billing"),
                    node("credit-card"),
                    node("customer"),
                    node("descriptor"),
                    node("shipping"),
                    node("subscription")
            );
        }

        private String SubscriptionXml(String id)
        {
            return node("subscription",
                    node("id", id),
                    node_attr("transactions", TYPE_ARRAY),
                    node_attr("add_ons", TYPE_ARRAY),
                    node_attr("discounts", TYPE_ARRAY)
            );
        }

        private String MerchantAccountApprovedSampleXml(String id)
        {
            return node("merchant-account",
                    node("id", id),
                    node("master-merchant-account",
                        node("id", "master_ma_for_" + id),
                        node("status", "active")
                    ),
                    node("status", "active")
            );
        }

        private String PartnerMerchantConnectedSampleXml(String id) {
            return node("partner-merchant",
                    node("partner-merchant-id", "abc123"),
                    node("merchant-public-id", "public_id"),
                    node("public-key", "public_key"),
                    node("private-key", "private_key"),
                    node("client-side-encryption-key", "cse_key")
            );
        }

        private String PartnerMerchantDisconnectedSampleXml(String id) {
            return node("partner-merchant",
                    node("partner-merchant-id", "abc123")
            );
        }

        private String PartnerMerchantDeclinedSampleXml(String id) {
            return node("partner-merchant",
                    node("partner-merchant-id", "abc123")
            );
        }

        private static string node(string name, params string[] contents) {
            return node_attr(name, null, contents);
        }

        private static string node_attr(string name, string attributes, params string[] contents) {
            StringBuilder buffer = new StringBuilder();
            buffer.Append('<').Append(name);
            if (attributes != null) {
                buffer.Append(" ").Append(attributes);
            }
            buffer.Append('>');
            foreach (string content in contents) {
                buffer.Append(content);
            }
            buffer.Append("</").Append(name).Append('>');
            return buffer.ToString();
        }
    }
}

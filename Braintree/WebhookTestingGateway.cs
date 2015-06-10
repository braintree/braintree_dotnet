#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class WebhookTestingGateway
    {
        private BraintreeService Service;

        protected internal WebhookTestingGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Service = new BraintreeService(gateway.Configuration);
        }

        public virtual Dictionary<string, string> SampleNotification(WebhookKind kind, string id)
        {
            var response = new Dictionary<string, string>();
            string payload = BuildPayload(kind, id);
            response["bt_payload"] = payload;
            response["bt_signature"] = BuildSignature(payload);
            return response;
        }

        private string BuildPayload(WebhookKind kind, string id)
        {
            var currentTime = DateTime.Now.ToUniversalTime().ToString("u");
            var payload = string.Format("<notification><timestamp type=\"datetime\">{0}</timestamp><kind>{1}</kind><subject>{2}</subject></notification>", currentTime, kind, SubjectSampleXml(kind, id));
            return Convert.ToBase64String(Encoding.Default.GetBytes(payload)) + '\n';
        }

        private string BuildSignature(string payload)
        {
            return string.Format("{0}|{1}", Service.PublicKey, new Sha1Hasher().HmacHash(Service.PrivateKey, payload).Trim().ToLower());
        }

        private string SubjectSampleXml(WebhookKind kind, string id)
        {
            if (kind == WebhookKind.SUB_MERCHANT_ACCOUNT_APPROVED) {
                return MerchantAccountApprovedSampleXml(id);
            } else if (kind == WebhookKind.SUB_MERCHANT_ACCOUNT_DECLINED) {
                return MerchantAccountDeclinedSampleXml(id);
            } else if (kind == WebhookKind.TRANSACTION_DISBURSED) {
                return TransactionDisbursedSampleXml(id);
            } else if (kind == WebhookKind.DISBURSEMENT_EXCEPTION) {
                return DisbursementExceptionSampleXml(id);
            } else if (kind == WebhookKind.DISBURSEMENT) {
                return DisbursementSampleXml(id);
            } else if (kind == WebhookKind.PARTNER_MERCHANT_CONNECTED) {
                return PartnerMerchantConnectedSampleXml(id);
            } else if (kind == WebhookKind.PARTNER_MERCHANT_DISCONNECTED) {
                return PartnerMerchantDisconnectedSampleXml(id);
            } else if (kind == WebhookKind.PARTNER_MERCHANT_DECLINED) {
                return PartnerMerchantDeclinedSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_OPENED) {
                return DisputeOpenedSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_LOST) {
                return DisputeLostSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_WON) {
                return DisputeWonSampleXml(id);
            } else {
                return SubscriptionXml(id);
            }
        }

        private static readonly string TYPE_DATE = "type=\"date\"";
        private static readonly string TYPE_ARRAY = "type=\"array\"";
        private static readonly string TYPE_SYMBOL = "type=\"symbol\"";
        private static readonly string NIL_TRUE = "nil=\"true\"";
        private static readonly string TYPE_BOOLEAN = "type=\"boolean\"";

        private string MerchantAccountDeclinedSampleXml(string id)
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

        private string TransactionDisbursedSampleXml(string id)
        {
            return node("transaction",
                    node("id", id),
                    node("amount", "100.00"),
                    node("disbursement-details",
                        node_attr("disbursement-date", TYPE_DATE, "2013-07-09")
                    ),
                    node("billing"),
                    node("credit-card"),
                    node("customer"),
                    node("descriptor"),
                    node("shipping"),
                    node("subscription")
            );
        }

        private string DisbursementExceptionSampleXml(string id)
        {
            return node("disbursement",
                    node("id", id),
                    node("amount", "100.00"),
                    node("exception-message", "bank_rejected"),
                    node_attr("disbursement-date", TYPE_DATE, "2014-02-10"),
                    node("follow-up-action", "update_funding_information"),
                    node_attr("success", TYPE_BOOLEAN, "false"),
                    node_attr("retry", TYPE_BOOLEAN, "false"),
                    node("merchant-account",
                        node("id", "merchant_account_id"),
                        node("master-merchant-account",
                            node("id", "master_ma"),
                            node("status", "active")
                        ),
                        node("status", "active")
                    ),
                    node_attr("transaction-ids", TYPE_ARRAY,
                        node("item", "asdf"),
                        node("item", "qwer")
                    )
            );
        }

        private string DisbursementSampleXml(string id)
        {
            return node("disbursement",
                    node("id", id),
                    node("amount", "100.00"),
                    node_attr("exception-message", NIL_TRUE, ""),
                    node_attr("disbursement-date", TYPE_DATE, "2014-02-10"),
                    node_attr("follow-up-action", NIL_TRUE, ""),
                    node_attr("success", TYPE_BOOLEAN, "true"),
                    node_attr("retry", TYPE_BOOLEAN, "false"),
                    node("merchant-account",
                        node("id", "merchant_account_id"),
                        node("master-merchant-account",
                            node("id", "master_ma"),
                            node("status", "active")
                        ),
                        node("status", "active")
                    ),
                    node_attr("transaction-ids", TYPE_ARRAY,
                        node("item", "asdf"),
                        node("item", "qwer")
                    )
            );
        }

        private string DisputeOpenedSampleXml(string id) {
            return node("dispute",
                    node("id", id),
                    node("amount", "250.00"),
                    node_attr("received-date", TYPE_DATE, "2014-03-21"),
                    node_attr("repy-by-date", TYPE_DATE, "2014-03-21"),
                    node("currency-iso-code", "USD"),
                    node("status", "open"),
                    node("reason", "fraud"),
                    node("transaction",
                        node("id", id),
                        node("amount", "250.00")
                    )
            );
        }

        private string DisputeLostSampleXml(string id) {
            return node("dispute",
                    node("id", id),
                    node("amount", "250.00"),
                    node_attr("received-date", TYPE_DATE, "2014-03-21"),
                    node_attr("repy-by-date", TYPE_DATE, "2014-03-21"),
                    node("currency-iso-code", "USD"),
                    node("status", "lost"),
                    node("reason", "fraud"),
                    node("transaction",
                        node("id", id),
                        node("amount", "250.00")
                    )
            );
        }

        private string DisputeWonSampleXml(string id) {
            return node("dispute",
                    node("id", id),
                    node("amount", "250.00"),
                    node_attr("received-date", TYPE_DATE, "2014-03-21"),
                    node_attr("repy-by-date", TYPE_DATE, "2014-03-21"),
                    node("currency-iso-code", "USD"),
                    node("status", "won"),
                    node("reason", "fraud"),
                    node("transaction",
                        node("id", id),
                        node("amount", "250.00")
                    )
            );
        }

        private string SubscriptionXml(string id)
        {
            return node("subscription",
                    node("id", id),
                    node_attr("transactions", TYPE_ARRAY),
                    node_attr("add_ons", TYPE_ARRAY),
                    node_attr("discounts", TYPE_ARRAY)
            );
        }

        private string MerchantAccountApprovedSampleXml(string id)
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

        private string PartnerMerchantConnectedSampleXml(string id) {
            return node("partner-merchant",
                    node("partner-merchant-id", "abc123"),
                    node("merchant-public-id", "public_id"),
                    node("public-key", "public_key"),
                    node("private-key", "private_key"),
                    node("client-side-encryption-key", "cse_key")
            );
        }

        private string PartnerMerchantDisconnectedSampleXml(string id) {
            return node("partner-merchant",
                    node("partner-merchant-id", "abc123")
            );
        }

        private string PartnerMerchantDeclinedSampleXml(string id) {
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

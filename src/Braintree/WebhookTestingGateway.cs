#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class WebhookTestingGateway : IWebhookTestingGateway
    {
        private readonly BraintreeService service;

        protected internal WebhookTestingGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            service = new BraintreeService(gateway.Configuration);
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
            return Convert.ToBase64String(Encoding.GetEncoding(0).GetBytes(payload)) + '\n';
        }

        private string BuildSignature(string payload)
        {
            return string.Format("{0}|{1}", service.PublicKey, new Sha1Hasher().HmacHash(service.PrivateKey, payload).Trim().ToLower());
        }

        private string SubjectSampleXml(WebhookKind kind, string id)
        {
            if (kind == WebhookKind.SUB_MERCHANT_ACCOUNT_APPROVED) {
                return MerchantAccountApprovedSampleXml(id);
            } else if (kind == WebhookKind.SUB_MERCHANT_ACCOUNT_DECLINED) {
                return MerchantAccountDeclinedSampleXml(id);
            } else if (kind == WebhookKind.TRANSACTION_DISBURSED) {
                return TransactionDisbursedSampleXml(id);
            } else if (kind == WebhookKind.TRANSACTION_SETTLED) {
                return TransactionSettledSampleXml(id);
            } else if (kind == WebhookKind.TRANSACTION_SETTLEMENT_DECLINED) {
                return TransactionSettlementDeclinedSampleXml(id);
            } else if (kind == WebhookKind.DISBURSEMENT_EXCEPTION) {
                return DisbursementExceptionSampleXml(id);
            } else if (kind == WebhookKind.DISBURSEMENT) {
                return DisbursementSampleXml(id);
            } else if (kind == WebhookKind.PARTNER_MERCHANT_CONNECTED) {
                return PartnerMerchantConnectedSampleXml(id);
            } else if (kind == WebhookKind.PARTNER_MERCHANT_DISCONNECTED) {
                return PartnerMerchantDisconnectedSampleXml(id);
            } else if (kind == WebhookKind.CONNECTED_MERCHANT_STATUS_TRANSITIONED) {
                return ConnectedMerchantStatusTransitionedSampleXml(id);
            } else if (kind == WebhookKind.CONNECTED_MERCHANT_PAYPAL_STATUS_CHANGED) {
                return ConnectedMerchantPayPalStatusChangedSampleXml(id);
            } else if (kind == WebhookKind.PARTNER_MERCHANT_DECLINED) {
                return PartnerMerchantDeclinedSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_OPENED) {
                return DisputeOpenedSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_LOST) {
                return DisputeLostSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_WON) {
                return DisputeWonSampleXml(id);
            } else if (kind == WebhookKind.SUBSCRIPTION_CHARGED_SUCCESSFULLY) {
                return SubscriptionChargedSuccessfullySampleXml(id);
            } else if (kind == WebhookKind.CHECK) {
                return CheckSampleXml();
            } else if (kind == WebhookKind.ACCOUNT_UPDATER_DAILY_REPORT) {
                return AccountUpdaterDailyReportSampleXml(id);
            } else if (kind == WebhookKind.IDEAL_PAYMENT_COMPLETE) {
                return IdealPaymentCompleteSampleXml(id);
            } else if (kind == WebhookKind.IDEAL_PAYMENT_FAILED) {
                return IdealPaymentFailedSampleXml(id);
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
            return Node("api-error-response",
                Node("message", "Applicant declined due to OFAC."),
                NodeAttr("errors", TYPE_ARRAY,
                    Node("merchant-account",
                        NodeAttr("errors", TYPE_ARRAY,
                            Node("error",
                                Node("code", "82621"),
                                Node("message", "Applicant declined due to OFAC."),
                                NodeAttr("attribute", TYPE_SYMBOL, "base")
                            )
                        )
                    )
                ),
                Node("merchant-account",
                    Node("id", id),
                    Node("status", "suspended"),
                    Node("master-merchant-account",
                        Node("id", "master_ma_for_" + id),
                        Node("status", "suspended")
                    )
                )
            );
        }

        private string TransactionDisbursedSampleXml(string id)
        {
            return Node("transaction",
                    Node("id", id),
                    Node("amount", "100.00"),
                    Node("disbursement-details",
                        NodeAttr("disbursement-date", TYPE_DATE, "2013-07-09")
                    ),
                    Node("billing"),
                    Node("credit-card"),
                    Node("customer"),
                    Node("descriptor"),
                    Node("shipping"),
                    Node("subscription")
            );
        }

        private string TransactionSettledSampleXml(string id)
        {
            return Node("transaction",
                    Node("id", id),
                    Node("status", "settled"),
                    Node("type", "sale"),
                    Node("currency-iso-code", "USD"),
                    Node("amount", "100.00"),
                    Node("us-bank-account",
                        Node("routing-number", "123456789"),
                        Node("last-4", "1234"),
                        Node("account-type", "checking"),
                        Node("account-holder-name", "Dan Schulman")),
                    Node("disbursement-details"),
                    Node("billing"),
                    Node("credit-card"),
                    Node("customer"),
                    Node("descriptor"),
                    Node("shipping"),
                    Node("subscription")
            );
        }

        private string TransactionSettlementDeclinedSampleXml(string id)
        {
            return Node("transaction",
                    Node("id", id),
                    Node("status", "settlement_declined"),
                    Node("type", "sale"),
                    Node("currency-iso-code", "USD"),
                    Node("amount", "100.00"),
                    Node("us-bank-account",
                        Node("routing-number", "123456789"),
                        Node("last-4", "1234"),
                        Node("account-type", "checking"),
                        Node("account-holder-name", "Dan Schulman")),
                    Node("disbursement-details"),
                    Node("billing"),
                    Node("credit-card"),
                    Node("customer"),
                    Node("descriptor"),
                    Node("shipping"),
                    Node("subscription")
            );
        }

        private string DisbursementExceptionSampleXml(string id)
        {
            return Node("disbursement",
                    Node("id", id),
                    Node("amount", "100.00"),
                    Node("exception-message", "bank_rejected"),
                    NodeAttr("disbursement-date", TYPE_DATE, "2014-02-10"),
                    Node("follow-up-action", "update_funding_information"),
                    NodeAttr("success", TYPE_BOOLEAN, "false"),
                    NodeAttr("retry", TYPE_BOOLEAN, "false"),
                    Node("merchant-account",
                        Node("id", "merchant_account_id"),
                        Node("master-merchant-account",
                            Node("id", "master_ma"),
                            Node("status", "active")
                        ),
                        Node("status", "active")
                    ),
                    NodeAttr("transaction-ids", TYPE_ARRAY,
                        Node("item", "asdf"),
                        Node("item", "qwer")
                    )
            );
        }

        private string DisbursementSampleXml(string id)
        {
            return Node("disbursement",
                    Node("id", id),
                    Node("amount", "100.00"),
                    NodeAttr("exception-message", NIL_TRUE, ""),
                    NodeAttr("disbursement-date", TYPE_DATE, "2014-02-10"),
                    NodeAttr("follow-up-action", NIL_TRUE, ""),
                    NodeAttr("success", TYPE_BOOLEAN, "true"),
                    NodeAttr("retry", TYPE_BOOLEAN, "false"),
                    Node("merchant-account",
                        Node("id", "merchant_account_id"),
                        Node("master-merchant-account",
                            Node("id", "master_ma"),
                            Node("status", "active")
                        ),
                        Node("status", "active")
                    ),
                    NodeAttr("transaction-ids", TYPE_ARRAY,
                        Node("item", "asdf"),
                        Node("item", "qwer")
                    )
            );
        }

        private string DisputeOpenedSampleXml(string id) {
            return Node("dispute",
                    Node("id", id),
                    Node("amount", "250.00"),
                    NodeAttr("received-date", TYPE_DATE, "2014-03-21"),
                    NodeAttr("reply-by-date", TYPE_DATE, "2014-03-21"),
                    Node("currency-iso-code", "USD"),
                    Node("kind", "chargeback"),
                    Node("status", "open"),
                    Node("reason", "fraud"),
                    Node("transaction",
                        Node("id", id),
                        Node("amount", "250.00")
                    ),
                    NodeAttr("date-opened", TYPE_DATE, "2014-03-21")
            );
        }

        private string DisputeLostSampleXml(string id) {
            return Node("dispute",
                    Node("id", id),
                    Node("amount", "250.00"),
                    NodeAttr("received-date", TYPE_DATE, "2014-03-21"),
                    NodeAttr("reply-by-date", TYPE_DATE, "2014-03-21"),
                    Node("currency-iso-code", "USD"),
                    Node("kind", "chargeback"),
                    Node("status", "lost"),
                    Node("reason", "fraud"),
                    Node("transaction",
                        Node("id", id),
                        Node("amount", "250.00")
                    ),
                    NodeAttr("date-opened", TYPE_DATE, "2014-03-21")
            );
        }

        private string DisputeWonSampleXml(string id) {
            return Node("dispute",
                    Node("id", id),
                    Node("amount", "250.00"),
                    NodeAttr("received-date", TYPE_DATE, "2014-03-21"),
                    NodeAttr("reply-by-date", TYPE_DATE, "2014-03-21"),
                    Node("currency-iso-code", "USD"),
                    Node("kind", "chargeback"),
                    Node("status", "won"),
                    Node("reason", "fraud"),
                    Node("transaction",
                        Node("id", id),
                        Node("amount", "250.00")
                    ),
                    NodeAttr("date-opened", TYPE_DATE, "2014-03-21"),
                    NodeAttr("date-won", TYPE_DATE, "2014-03-22")
            );
        }

        private string SubscriptionXml(string id)
        {
            return Node("subscription",
                    Node("id", id),
                    NodeAttr("transactions", TYPE_ARRAY),
                    NodeAttr("add_ons", TYPE_ARRAY),
                    NodeAttr("discounts", TYPE_ARRAY)
            );
        }

        private string SubscriptionChargedSuccessfullySampleXml(string id)
        {
            return Node("subscription",
                    Node("id", id),
                    Node("transactions",
                        Node("transaction",
                            Node("id", id),
                            Node("amount", "49.99"),
                            Node("status", "submitted_for_settlement"),
                            Node("disbursement-details",
                                NodeAttr("disbursement-date", TYPE_DATE, "2013-07-09")
                            ),
                            Node("billing"),
                            Node("credit-card"),
                            Node("customer"),
                            Node("descriptor"),
                            Node("shipping"),
                            Node("subscription")
                        )
                    ),
                    NodeAttr("add_ons", TYPE_ARRAY),
                    NodeAttr("discounts", TYPE_ARRAY)
            );
        }

        private string CheckSampleXml()
        {
            return NodeAttr("check", TYPE_BOOLEAN, "true");
        }

        private string MerchantAccountApprovedSampleXml(string id)
        {
            return Node("merchant-account",
                    Node("id", id),
                    Node("master-merchant-account",
                        Node("id", "master_ma_for_" + id),
                        Node("status", "active")
                    ),
                    Node("status", "active")
            );
        }

        private string PartnerMerchantConnectedSampleXml(string id) {
            return Node("partner-merchant",
                    Node("partner-merchant-id", "abc123"),
                    Node("merchant-public-id", "public_id"),
                    Node("public-key", "public_key"),
                    Node("private-key", "private_key"),
                    Node("client-side-encryption-key", "cse_key")
            );
        }

        private string PartnerMerchantDisconnectedSampleXml(string id) {
            return Node("partner-merchant",
                    Node("partner-merchant-id", "abc123")
            );
        }

        private string ConnectedMerchantStatusTransitionedSampleXml(string id) {
            return Node("connected-merchant-status-transitioned",
                    Node("oauth-application-client-id", "oauth_application_client_id"),
                    Node("merchant-public-id", id),
                    Node("status", "new_status")
                   );
        }

        private string ConnectedMerchantPayPalStatusChangedSampleXml(string id) {
            return Node("connected-merchant-paypal-status-changed",
                    Node("oauth-application-client-id", "oauth_application_client_id"),
                    Node("merchant-public-id", id),
                    Node("action", "link")
                   );
        }

        private string PartnerMerchantDeclinedSampleXml(string id) {
            return Node("partner-merchant",
                    Node("partner-merchant-id", "abc123")
            );
        }

        private string AccountUpdaterDailyReportSampleXml(string id) {
            return Node("account-updater-daily-report",
                    NodeAttr("report-date", TYPE_DATE, "2016-01-14"),
                    Node("report-url", "link-to-csv-report")
            );
        }

        private string IdealPaymentCompleteSampleXml(string id) {
            return Node("ideal-payment",
                    Node("id", id),
                    Node("status", "COMPLETE"),
                    Node("issuer", "ABCISSUER"),
                    Node("order-id", "ORDERABC"),
                    Node("currency", "EUR"),
                    Node("amount", "10.00"),
                    Node("created-at", "2016-11-29T23:27:34.547Z"),
                    Node("approval-url", "https://example.com"),
                    Node("ideal-transaction-id", "1234567890")
            );
        }

        private string IdealPaymentFailedSampleXml(string id) {
            return Node("ideal-payment",
                    Node("id", id),
                    Node("status", "FAILED"),
                    Node("issuer", "ABCISSUER"),
                    Node("order-id", "ORDERABC"),
                    Node("currency", "EUR"),
                    Node("amount", "10.00"),
                    Node("created-at", "2016-11-29T23:27:34.547Z"),
                    Node("approval-url", "https://example.com"),
                    Node("ideal-transaction-id", "1234567890")
            );
        }

        private static string Node(string name, params string[] contents) {
            return NodeAttr(name, null, contents);
        }

        private static string NodeAttr(string name, string attributes, params string[] contents) {
            StringBuilder buffer = new StringBuilder();
            buffer.Append('<').Append(name);
            if (attributes != null)
            {
                buffer.Append(" ").Append(attributes);
            }
            buffer.Append('>');
            foreach (string content in contents)
            {
                buffer.Append(content);
            }
            buffer.Append("</").Append(name).Append('>');
            return buffer.ToString();
        }
    }
}

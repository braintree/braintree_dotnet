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
            service = gateway.Service;
        }

        public virtual Dictionary<string, string> SampleNotification(WebhookKind kind, string id, string sourceMerchantId = null)
        {
            var response = new Dictionary<string, string>();
            string payload = BuildPayload(kind, id, sourceMerchantId);
            response["bt_payload"] = payload;
            response["bt_signature"] = BuildSignature(payload);
            return response;
        }

        private string BuildPayload(WebhookKind kind, string id, string sourceMerchantId)
        {
            var currentTime = DateTime.Now.ToUniversalTime().ToString("u");
            var sourceMerchantIdXml = "";
            if (sourceMerchantId != null) {
                sourceMerchantIdXml = $"<source-merchant-id>{sourceMerchantId}</source-merchant-id>";
            }
            var payload =
                $"<notification><timestamp type=\"datetime\">{currentTime}</timestamp><kind>{kind}</kind>{sourceMerchantIdXml}<subject>{SubjectSampleXml(kind, id)}</subject></notification>";
            return Convert.ToBase64String(Encoding.GetEncoding(0).GetBytes(payload)) + '\n';
        }

        private string BuildSignature(string payload)
        {
            return $"{service.PublicKey}|{new Sha1Hasher().HmacHash(service.PrivateKey, payload).Trim().ToLower()}";
        }

        private string SubjectSampleXml(WebhookKind kind, string id)
        {
            // NEXT_UNDER_MAJOR_VERSION
            // Convert to switch statement
            if (kind == WebhookKind.SUB_MERCHANT_ACCOUNT_APPROVED) {
                return MerchantAccountApprovedSampleXml(id);
            } else if (kind == WebhookKind.SUB_MERCHANT_ACCOUNT_DECLINED) {
                return MerchantAccountDeclinedSampleXml(id);
            } else if (kind == WebhookKind.TRANSACTION_DISBURSED) {
                return TransactionDisbursedSampleXml(id);
            } else if (kind == WebhookKind.TRANSACTION_REVIEWED) {
                return TransactionReviewedSampleXml(id);
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
            } else if (kind == WebhookKind.OAUTH_ACCESS_REVOKED) {
                return OAuthAccessRevokedSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_OPENED) {
                return DisputeOpenedSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_LOST) {
                return DisputeLostSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_WON) {
                return DisputeWonSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_ACCEPTED) {
                return DisputeAcceptedSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_AUTO_ACCEPTED) {
                return DisputeAutoAcceptedSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_DISPUTED) {
                return DisputeDisputedSampleXml(id);
            } else if (kind == WebhookKind.DISPUTE_EXPIRED) {
                return DisputeExpiredSampleXml(id);
            } else if (kind == WebhookKind.SUBSCRIPTION_BILLING_SKIPPED) {
                return SubscriptionBillingSkippedSampleXml(id);
            } else if (kind == WebhookKind.SUBSCRIPTION_CHARGED_SUCCESSFULLY) {
                return SubscriptionChargedSuccessfullySampleXml(id);
            } else if (kind == WebhookKind.SUBSCRIPTION_CHARGED_UNSUCCESSFULLY) {
                return SubscriptionChargedUnsuccessfullySampleXml(id);
            } else if (kind == WebhookKind.CHECK) {
                return CheckSampleXml();
            } else if (kind == WebhookKind.ACCOUNT_UPDATER_DAILY_REPORT) {
                return AccountUpdaterDailyReportSampleXml(id);
            } else if (kind == WebhookKind.GRANTOR_UPDATED_GRANTED_PAYMENT_METHOD) {
                return GrantedPaymentInstrumentUpdateSampleXml();
            } else if (kind == WebhookKind.RECIPIENT_UPDATED_GRANTED_PAYMENT_METHOD) {
                return GrantedPaymentInstrumentUpdateSampleXml();
            } else if (kind == WebhookKind.PAYMENT_METHOD_REVOKED_BY_CUSTOMER) {
                return PaymentMethodRevokedByCustomerSampleXml(id);
            } else if (kind == WebhookKind.GRANTED_PAYMENT_METHOD_REVOKED) {
                return GrantedPaymentMethodRevokedSampleXml(id);
            } else if (kind == WebhookKind.LOCAL_PAYMENT_COMPLETED) {
                return LocalPaymentCompletedSampleXml();
            } else if (kind == WebhookKind.LOCAL_PAYMENT_EXPIRED) {
                return LocalPaymentExpiredSampleXml();
            } else if (kind == WebhookKind.LOCAL_PAYMENT_FUNDED) {
                return LocalPaymentFundedSampleXml();
            } else if (kind == WebhookKind.LOCAL_PAYMENT_REVERSED) {
                return LocalPaymentReversedSampleXml();
            } else if (kind == WebhookKind.PAYMENT_METHOD_CUSTOMER_DATA_UPDATED) {
                return PaymentMethodCustomerDataUpdatedMetadataSampleXml(id);
            } else {
                return SubscriptionXml(id);
            }
        }

        private static readonly string TYPE_DATE = "type=\"date\"";
        private static readonly string TYPE_DATE_TIME = "type=\"datetime\"";
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

        private string TransactionReviewedSampleXml(string id)
        {
            return Node("transaction-review",
                    Node("transaction-id", "my_id"),
                    Node("decision", "smart_decision"),
                    Node("reviewer-email", "hey@girl.com"),
                    Node("reviewer-note", "I made a smart decision."),
                    NodeAttr("reviewed-time", TYPE_DATE_TIME, "2019-01-02T00:00:00Z")
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
                    Node("amount-disputed", "250.00"),
                    Node("amount-won", "245.00"),
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
                    Node("amount-disputed", "250.00"),
                    Node("amount-won", "245.00"),
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
                    Node("amount-disputed", "250.00"),
                    Node("amount-won", "245.00"),
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

        private string DisputeAcceptedSampleXml(string id) {
            return Node("dispute",
                    Node("id", id),
                    Node("amount", "250.00"),
                    Node("amount-disputed", "250.00"),
                    Node("amount-won", "245.00"),
                    NodeAttr("received-date", TYPE_DATE, "2014-03-21"),
                    NodeAttr("reply-by-date", TYPE_DATE, "2014-03-21"),
                    Node("currency-iso-code", "USD"),
                    Node("kind", "chargeback"),
                    Node("status", "accepted"),
                    Node("reason", "fraud"),
                    Node("transaction",
                        Node("id", id),
                        Node("amount", "250.00")
                    ),
                    NodeAttr("date-opened", TYPE_DATE, "2014-03-21")
            );
        }

        private string DisputeAutoAcceptedSampleXml(string id) {
            return Node("dispute",
                    Node("id", id),
                    Node("amount", "250.00"),
                    Node("amount-disputed", "250.00"),
                    Node("amount-won", "245.00"),
                    NodeAttr("received-date", TYPE_DATE, "2014-03-21"),
                    NodeAttr("reply-by-date", TYPE_DATE, "2014-03-21"),
                    Node("currency-iso-code", "USD"),
                    Node("kind", "chargeback"),
                    Node("status", "auto_accepted"),
                    Node("reason", "fraud"),
                    Node("transaction",
                        Node("id", id),
                        Node("amount", "250.00")
                    ),
                    NodeAttr("date-opened", TYPE_DATE, "2014-03-21")
            );
        }

        private string DisputeDisputedSampleXml(string id) {
            return Node("dispute",
                    Node("id", id),
                    Node("amount", "250.00"),
                    Node("amount-disputed", "250.00"),
                    Node("amount-won", "245.00"),
                    NodeAttr("received-date", TYPE_DATE, "2014-03-21"),
                    NodeAttr("reply-by-date", TYPE_DATE, "2014-03-21"),
                    Node("currency-iso-code", "USD"),
                    Node("kind", "chargeback"),
                    Node("status", "disputed"),
                    Node("reason", "fraud"),
                    Node("transaction",
                        Node("id", id),
                        Node("amount", "250.00")
                    ),
                    NodeAttr("date-opened", TYPE_DATE, "2014-03-21")
            );
        }

        private string DisputeExpiredSampleXml(string id) {
            return Node("dispute",
                    Node("id", id),
                    Node("amount", "250.00"),
                    Node("amount-disputed", "250.00"),
                    Node("amount-won", "245.00"),
                    NodeAttr("received-date", TYPE_DATE, "2014-03-21"),
                    NodeAttr("reply-by-date", TYPE_DATE, "2014-03-21"),
                    Node("currency-iso-code", "USD"),
                    Node("kind", "chargeback"),
                    Node("status", "expired"),
                    Node("reason", "fraud"),
                    Node("transaction",
                        Node("id", id),
                        Node("amount", "250.00")
                    ),
                    NodeAttr("date-opened", TYPE_DATE, "2014-03-21")
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

        private string SubscriptionBillingSkippedSampleXml(string id)
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

        private string SubscriptionChargedUnsuccessfullySampleXml(string id)
        {
            return Node("subscription",
                    Node("id", id),
                    Node("transactions",
                        Node("transaction",
                            Node("id", id),
                            Node("amount", "49.99"),
                            Node("status", "failed"),
                            Node("disbursement-details"),
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

        private string OAuthAccessRevokedSampleXml(string id) {
            return Node("oauth-application-revocation",
                    Node("merchant-id", id),
                    Node("oauth-application-client-id", "oauth_application_client_id")
            );
        }

        private string AccountUpdaterDailyReportSampleXml(string id) {
            return Node("account-updater-daily-report",
                    NodeAttr("report-date", TYPE_DATE, "2016-01-14"),
                    Node("report-url", "link-to-csv-report")
            );
        }

        private string GrantedPaymentInstrumentUpdateSampleXml() {
            return Node("granted-payment-instrument-update",
                    Node("grant-owner-merchant-id", "vczo7jqrpwrsi2px"),
                    Node("grant-recipient-merchant-id", "cf0i8wgarszuy6hc"),
                    Node("payment-method-nonce",
                        Node("nonce", "ee257d98-de40-47e8-96b3-a6954ea7a9a4"),
                        Node("consumed", TYPE_BOOLEAN, "false"),
                        Node("locked", TYPE_BOOLEAN, "false")
                        ),
                    Node("token", "abc123z"),
                    Node("updated-fields", TYPE_ARRAY,
                        Node("item", "expiration-month"),
                        Node("item", "expiration-year")
                        )
            );
        }

        private static string PaymentMethodRevokedByCustomerSampleXml(string id) {
            return Node("paypal-account",
                    Node("billing-agreement-id", "a-billing-agreement-id"),
                    NodeAttr("created-at", TYPE_DATE_TIME, "2019-01-01T12:00:00Z"),
                    Node("customer-id", "a-customer-id"),
                    NodeAttr("default", TYPE_BOOLEAN, "true"),
                    Node("email", "name@email.com"),
                    Node("global-id", "cGF5bWVudG1ldGhvZF9jaDZieXNz"),
                    Node("image-url", "https://assets.braintreegateway.com/payment_method_logo/paypal.png?environment=test"),
                    Node("token", id),
                    NodeAttr("updated-at", TYPE_DATE_TIME, "2019-01-02T12:00:00Z"),
                    Node("is-channel-initiated", NIL_TRUE, ""),
                    Node("payer-id", "a-payer-id"),
                    Node("payer-info", NIL_TRUE, ""),
                    Node("limited-use-order-id", NIL_TRUE, ""),
                    NodeAttr("revoked-at", TYPE_DATE_TIME, "2019-01-02T12:00:00Z")
                );
        }

        private static string GrantedPaymentMethodRevokedSampleXml(string id) {
            return VenmoAccountSampleXml(id);
        }

        private static string LocalPaymentCompletedSampleXml() {
            return Node("local-payment",
                    Node("payment-id", "a-payment-id"),
                    Node("payer-id", "a-payer-id"),
                    Node("payment-method-nonce", "ee257d98-de40-47e8-96b3-a6954ea7a9a4"),
                    Node("transaction",
                        Node("id", "1"),
                        Node("status", "authorizing"),
                        Node("amount", "10.00"),
                        Node("order-id", "order1234")
                        )
            );
        }

        private static string LocalPaymentExpiredSampleXml() {
            return Node("local-payment-expired",
                    Node("payment-id", "a-payment-id"),
                    Node("payment-context-id", "a-payment-context-id")
            );
        }

        private static string LocalPaymentFundedSampleXml() {
            return Node("local-payment-funded",
                    Node("payment-id", "a-payment-id"),
                    Node("payment-context-id", "a-payment-context-id"),
                    Node("transaction",
                        Node("id", "1"),
                        Node("status", "settled"),
                        Node("amount", "10.00"),
                        Node("order-id", "order1234")
                        )
            );
        }

        private static string LocalPaymentReversedSampleXml() {
            return Node("local-payment-reversed",
                    Node("payment-id", "a-payment-id")
            );
        }

        private static string PaymentMethodCustomerDataUpdatedMetadataSampleXml(string id) {
            return Node("payment-method-customer-data-updated-metadata",
                    Node("token", "TOKEN12345"),
                    Node("payment-method", VenmoAccountSampleXml(id)),
                    NodeAttr("datetime-updated", TYPE_DATE_TIME, "2022-01-01T21:28:37Z"),
                    Node("enriched-customer-data",
                        Node("fields-updated", TYPE_ARRAY,
                            Node("item", "username")
                        ),
                        Node("profile-data",
                            Node("first-name", "John"),
                            Node("last-name", "Doe"),
                            Node("phone-number", "1231231234"),
                            Node("email", "john.doe@paypal.com"),
                            Node("username", "venmo_username"),
                            Node("billing-address",
                                Node("street-address", "billing-street-address"),
                                Node("extended-address", "billing-extended-address"),
                                Node("locality", "billing-locality"),
                                Node("region", "billing-region"),
                                Node("postal-code", "billing-code")
                            ),
                            Node("shipping-address",
                                Node("street-address", "shipping-street-address"),
                                Node("extended-address", "shipping-extended-address"),
                                Node("locality", "shipping-locality"),
                                Node("region", "shipping-region"),
                                Node("postal-code", "shipping-code")
                            )
                        )
                    )
            );
        }

        private static string VenmoAccountSampleXml(string id) {
            return Node("venmo-account",
                    NodeAttr("created-at", TYPE_DATE_TIME, "2021-05-05T21:28:37Z"),
                    NodeAttr("updated-at", TYPE_DATE_TIME, "2021-05-05T21:28:37Z"),
                    NodeAttr("default", TYPE_BOOLEAN, "true"),
                    Node("image-url", "https://assets.braintreegateway.com/payment_method_logo/venmo.png?environment=test"),
                    Node("token", id),
                    Node("source-description", "Venmo Account: venmojoe"),
                    Node("username", "venmojoe"),
                    Node("venmo-user-id", "456"),
                    NodeAttr("subscriptions", TYPE_ARRAY),
                    Node("customer-id", "venmo_customer_id"),
                    Node("global-id", "cGF5bWVudG1ldGhvZF92ZW5tb2FjY291bnQ")
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

#pragma warning disable 1591

using System;

namespace Braintree
{
    public class TransactionSearchRequest : SearchRequest
    {
        public static string ACH_ANY_REASON_CODE = "any_reason_code";
        
        public RangeNode<TransactionSearchRequest> Amount => new RangeNode<TransactionSearchRequest>("amount", this);

        public DateRangeNode<TransactionSearchRequest> AuthorizationExpiredAt => new DateRangeNode<TransactionSearchRequest>("authorization-expired-at", this);

        public DateRangeNode<TransactionSearchRequest> AuthorizedAt => new DateRangeNode<TransactionSearchRequest>("authorized-at", this);

        public TextNode<TransactionSearchRequest> BillingCompany => new TextNode<TransactionSearchRequest>("billing-company", this);

        public TextNode<TransactionSearchRequest> BillingCountryName => new TextNode<TransactionSearchRequest>("billing-country-name", this);

        public TextNode<TransactionSearchRequest> BillingExtendedAddress => new TextNode<TransactionSearchRequest>("billing-extended-address", this);

        public TextNode<TransactionSearchRequest> BillingFirstName => new TextNode<TransactionSearchRequest>("billing-first-name", this);

        public TextNode<TransactionSearchRequest> BillingLastName => new TextNode<TransactionSearchRequest>("billing-last-name", this);

        public TextNode<TransactionSearchRequest> BillingLocality => new TextNode<TransactionSearchRequest>("billing-locality", this);

        public TextNode<TransactionSearchRequest> BillingPostalCode => new TextNode<TransactionSearchRequest>("billing-postal-code", this);

        public TextNode<TransactionSearchRequest> BillingRegion => new TextNode<TransactionSearchRequest>("billing-region", this);

        public TextNode<TransactionSearchRequest> BillingStreetAddress => new TextNode<TransactionSearchRequest>("billing-street-address", this);

        public DateRangeNode<TransactionSearchRequest> CreatedAt => new DateRangeNode<TransactionSearchRequest>("created-at", this);

        public EnumMultipleValueNode<TransactionSearchRequest, TransactionCreatedUsing> CreatedUsing => new EnumMultipleValueNode<TransactionSearchRequest, TransactionCreatedUsing>("created-using", this);

        public TextNode<TransactionSearchRequest> CreditCardCardholderName => new TextNode<TransactionSearchRequest>("credit-card-cardholder-name", this);

        public TextNode<TransactionSearchRequest> User => new TextNode<TransactionSearchRequest>("user", this);

        public TextNode<TransactionSearchRequest> CreditCardUniqueIdentifier => new TextNode<TransactionSearchRequest>("credit-card-unique-identifier", this);

        public EqualityNode<TransactionSearchRequest> CreditCardExpirationDate => new EqualityNode<TransactionSearchRequest>("credit-card-expiration-date", this);

        public PartialMatchNode<TransactionSearchRequest> CreditCardNumber => new PartialMatchNode<TransactionSearchRequest>("credit-card-number", this);

        public EnumMultipleValueNode<TransactionSearchRequest, Braintree.CreditCardCardType> CreditCardCardType => new EnumMultipleValueNode<TransactionSearchRequest, Braintree.CreditCardCardType>("credit-card-card-type", this);

        public MultipleValueNode<TransactionSearchRequest, String> PaymentInstrumentType => new MultipleValueNode<TransactionSearchRequest, String>("payment-instrument-type", this);

        public EnumMultipleValueNode<TransactionSearchRequest, Braintree.CreditCardCustomerLocation> CreditCardCustomerLocation => new EnumMultipleValueNode<TransactionSearchRequest, Braintree.CreditCardCustomerLocation>("credit-card-customer-location", this);

        public TextNode<TransactionSearchRequest> Currency => new TextNode<TransactionSearchRequest>("currency", this);

        public TextNode<TransactionSearchRequest> CustomerCompany => new TextNode<TransactionSearchRequest>("customer-company", this);

        public TextNode<TransactionSearchRequest> CustomerEmail => new TextNode<TransactionSearchRequest>("customer-email", this);

        public TextNode<TransactionSearchRequest> CustomerFax => new TextNode<TransactionSearchRequest>("customer-fax", this);

        public TextNode<TransactionSearchRequest> CustomerFirstName => new TextNode<TransactionSearchRequest>("customer-first-name", this);

        public TextNode<TransactionSearchRequest> CustomerId => new TextNode<TransactionSearchRequest>("customer-id", this);

        public TextNode<TransactionSearchRequest> CustomerLastName => new TextNode<TransactionSearchRequest>("customer-last-name", this);

        public TextNode<TransactionSearchRequest> CustomerPhone => new TextNode<TransactionSearchRequest>("customer-phone", this);

        public TextNode<TransactionSearchRequest> CustomerWebsite => new TextNode<TransactionSearchRequest>("customer-website", this);

        public EnumMultipleValueNode<TransactionSearchRequest, Braintree.TransactionDebitNetwork> DebitNetwork => new EnumMultipleValueNode<TransactionSearchRequest, Braintree.TransactionDebitNetwork>("debit-network", this);
        
        public DateRangeNode<TransactionSearchRequest> DisbursementDate => new DateRangeNode<TransactionSearchRequest>("disbursement-date", this);

        public DateRangeNode<TransactionSearchRequest> DisputeDate => new DateRangeNode<TransactionSearchRequest>("dispute-date", this);

        public DateRangeNode<TransactionSearchRequest> FailedAt => new DateRangeNode<TransactionSearchRequest>("failed-at", this);

        public DateRangeNode<TransactionSearchRequest> GatewayRejectedAt => new DateRangeNode<TransactionSearchRequest>("gateway-rejected-at", this);

        public TextNode<TransactionSearchRequest> Id => new TextNode<TransactionSearchRequest>("id", this);

        public MultipleValueNode<TransactionSearchRequest, string> Ids => new MultipleValueNode<TransactionSearchRequest, string>("ids", this);

        public MultipleValueNode<TransactionSearchRequest, string> MerchantAccountId => new MultipleValueNode<TransactionSearchRequest, string>("merchant-account-id", this);

        public TextNode<TransactionSearchRequest> OrderId => new TextNode<TransactionSearchRequest>("order-id", this);

        public TextNode<TransactionSearchRequest> PayPalAuthorizationId => new TextNode<TransactionSearchRequest>("paypal-authorization-id", this);

        public TextNode<TransactionSearchRequest> PayPalPayerEmail => new TextNode<TransactionSearchRequest>("paypal-payer-email", this);

        public TextNode<TransactionSearchRequest> PayPalPaymentId => new TextNode<TransactionSearchRequest>("paypal-payment-id", this);

        public TextNode<TransactionSearchRequest> SepaDebitPayPalV2OrderId => new TextNode<TransactionSearchRequest>("sepa_debit_paypal_v2_order_id", this);

        public TextNode<TransactionSearchRequest> PaymentMethodToken => new TextNode<TransactionSearchRequest>("payment-method-token", this);

        public TextNode<TransactionSearchRequest> ProcessorAuthorizationCode => new TextNode<TransactionSearchRequest>("processor-authorization-code", this);

        public DateRangeNode<TransactionSearchRequest> ProcessorDeclinedAt => new DateRangeNode<TransactionSearchRequest>("processor-declined-at", this);

        public KeyValueNode<TransactionSearchRequest> Refund => new KeyValueNode<TransactionSearchRequest>("refund", this);

        public DateRangeNode<TransactionSearchRequest> SettledAt => new DateRangeNode<TransactionSearchRequest>("settled-at", this);

        public TextNode<TransactionSearchRequest> SettlementBatchId => new TextNode<TransactionSearchRequest>("settlement-batch-id", this);

        public TextNode<TransactionSearchRequest> ShippingCompany => new TextNode<TransactionSearchRequest>("shipping-company", this);

        public TextNode<TransactionSearchRequest> ShippingCountryName => new TextNode<TransactionSearchRequest>("shipping-country-name", this);

        public TextNode<TransactionSearchRequest> ShippingExtendedAddress => new TextNode<TransactionSearchRequest>("shipping-extended-address", this);

        public TextNode<TransactionSearchRequest> ShippingFirstName => new TextNode<TransactionSearchRequest>("shipping-first-name", this);

        public TextNode<TransactionSearchRequest> ShippingLastName => new TextNode<TransactionSearchRequest>("shipping-last-name", this);

        public TextNode<TransactionSearchRequest> ShippingLocality => new TextNode<TransactionSearchRequest>("shipping-locality", this);

        public TextNode<TransactionSearchRequest> ShippingPostalCode => new TextNode<TransactionSearchRequest>("shipping-postal-code", this);

        public TextNode<TransactionSearchRequest> ShippingRegion => new TextNode<TransactionSearchRequest>("shipping-region", this);

        public TextNode<TransactionSearchRequest> ShippingStreetAddress => new TextNode<TransactionSearchRequest>("shipping-street-address", this);

        public EnumMultipleValueNode<TransactionSearchRequest, TransactionStatus> Status => new EnumMultipleValueNode<TransactionSearchRequest, TransactionStatus>("status", this);

        public DateRangeNode<TransactionSearchRequest> SubmittedForSettlementAt => new DateRangeNode<TransactionSearchRequest>("submitted-for-settlement-at", this);

        public EnumMultipleValueNode<TransactionSearchRequest, TransactionSource> Source => new EnumMultipleValueNode<TransactionSearchRequest, TransactionSource>("source", this);

        public TextNode<TransactionSearchRequest> StoreId => new TextNode<TransactionSearchRequest>("store-id", this);

        public MultipleValueNode<TransactionSearchRequest, string> StoreIds => new MultipleValueNode<TransactionSearchRequest, string>("store-ids", this);

        public EnumMultipleValueNode<TransactionSearchRequest, TransactionType> Type => new EnumMultipleValueNode<TransactionSearchRequest, TransactionType>("type", this);

        public DateRangeNode<TransactionSearchRequest> VoidedAt => new DateRangeNode<TransactionSearchRequest>("voided-at", this);

        public DateRangeNode<TransactionSearchRequest> AchReturnResponsesCreatedAt => new DateRangeNode<TransactionSearchRequest>("ach-return-responses-created-at", this);

        public MultipleValueNode<TransactionSearchRequest, string> ReasonCode => new MultipleValueNode<TransactionSearchRequest, string>("reason-code", this);

        public TransactionSearchRequest() : base()
        {
        }
    }
}

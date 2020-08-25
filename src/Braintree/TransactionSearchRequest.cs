#pragma warning disable 1591

using System;

namespace Braintree
{
    public class TransactionSearchRequest : SearchRequest
    {
        public RangeNode<TransactionSearchRequest> Amount
        {
            get
            {
                return new RangeNode<TransactionSearchRequest>("amount", this);
            }
        }
        public DateRangeNode<TransactionSearchRequest> AuthorizationExpiredAt
        {
            get
            {
                return new DateRangeNode<TransactionSearchRequest>("authorization-expired-at", this);
            }
        }
        public DateRangeNode<TransactionSearchRequest> AuthorizedAt
        {
            get
            {
                return new DateRangeNode<TransactionSearchRequest>("authorized-at", this);
            }
        }
        public TextNode<TransactionSearchRequest> BillingCompany
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("billing-company", this);
            }
        }
        public TextNode<TransactionSearchRequest> BillingCountryName
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("billing-country-name", this);
            }
        }
        public TextNode<TransactionSearchRequest> BillingExtendedAddress
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("billing-extended-address", this);
            }
        }
        public TextNode<TransactionSearchRequest> BillingFirstName
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("billing-first-name", this);
            }
        }
        public TextNode<TransactionSearchRequest> BillingLastName
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("billing-last-name", this);
            }
        }
        public TextNode<TransactionSearchRequest> BillingLocality
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("billing-locality", this);
            }
        }
        public TextNode<TransactionSearchRequest> BillingPostalCode
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("billing-postal-code", this);
            }
        }
        public TextNode<TransactionSearchRequest> BillingRegion
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("billing-region", this);
            }
        }
        public TextNode<TransactionSearchRequest> BillingStreetAddress
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("billing-street-address", this);
            }
        }
        public DateRangeNode<TransactionSearchRequest> CreatedAt
        {
            get
            {
                return new DateRangeNode<TransactionSearchRequest>("created-at", this);
            }
        }
        public EnumMultipleValueNode<TransactionSearchRequest, TransactionCreatedUsing> CreatedUsing
        {
            get
            {
                return new EnumMultipleValueNode<TransactionSearchRequest, TransactionCreatedUsing>("created-using", this);
            }
        }
        public TextNode<TransactionSearchRequest> CreditCardCardholderName
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("credit-card-cardholder-name", this);
            }
        }
        public TextNode<TransactionSearchRequest> User
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("user", this);
            }
        }
        public TextNode<TransactionSearchRequest> CreditCardUniqueIdentifier
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("credit-card-unique-identifier", this);
            }
        }
        public EqualityNode<TransactionSearchRequest> CreditCardExpirationDate
        {
            get
            {
                return new EqualityNode<TransactionSearchRequest>("credit-card-expiration-date", this);
            }
        }
        public PartialMatchNode<TransactionSearchRequest> CreditCardNumber
        {
            get
            {
                return new PartialMatchNode<TransactionSearchRequest>("credit-card-number", this);
            }
        }
        public EnumMultipleValueNode<TransactionSearchRequest, Braintree.CreditCardCardType> CreditCardCardType
        {
            get
            {
                return new EnumMultipleValueNode<TransactionSearchRequest, Braintree.CreditCardCardType>("credit-card-card-type", this);
            }
        }
        public MultipleValueNode<TransactionSearchRequest, String> PaymentInstrumentType
        {
            get
            {
                return new MultipleValueNode<TransactionSearchRequest, String>("payment-instrument-type", this);
            }
        }
        public EnumMultipleValueNode<TransactionSearchRequest, Braintree.CreditCardCustomerLocation> CreditCardCustomerLocation
        {
            get
            {
                return new EnumMultipleValueNode<TransactionSearchRequest, Braintree.CreditCardCustomerLocation>("credit-card-customer-location", this);
            }
        }
        public TextNode<TransactionSearchRequest> Currency
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("currency", this);
            }
        }
        public TextNode<TransactionSearchRequest> CustomerCompany
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("customer-company", this);
            }
        }
        public TextNode<TransactionSearchRequest> CustomerEmail
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("customer-email", this);
            }
        }
        public TextNode<TransactionSearchRequest> CustomerFax
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("customer-fax", this);
            }
        }
        public TextNode<TransactionSearchRequest> CustomerFirstName
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("customer-first-name", this);
            }
        }
        public TextNode<TransactionSearchRequest> CustomerId
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("customer-id", this);
            }
        }
        public TextNode<TransactionSearchRequest> CustomerLastName
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("customer-last-name", this);
            }
        }
        public TextNode<TransactionSearchRequest> CustomerPhone
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("customer-phone", this);
            }
        }
        public TextNode<TransactionSearchRequest> CustomerWebsite
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("customer-website", this);
            }
        }
        public DateRangeNode<TransactionSearchRequest> DisbursementDate
        {
            get
            {
                return new DateRangeNode<TransactionSearchRequest>("disbursement-date", this);
            }
        }
        public DateRangeNode<TransactionSearchRequest> DisputeDate
        {
            get
            {
                return new DateRangeNode<TransactionSearchRequest>("dispute-date", this);
            }
        }
        public DateRangeNode<TransactionSearchRequest> FailedAt
        {
            get
            {
                return new DateRangeNode<TransactionSearchRequest>("failed-at", this);
            }
        }
        public DateRangeNode<TransactionSearchRequest> GatewayRejectedAt
        {
            get
            {
                return new DateRangeNode<TransactionSearchRequest>("gateway-rejected-at", this);
            }
        }
        public TextNode<TransactionSearchRequest> Id
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("id", this);
            }
        }
        public MultipleValueNode<TransactionSearchRequest, string> Ids
        {
            get
            {
                return new MultipleValueNode<TransactionSearchRequest, string>("ids", this);
            }
        }
        public MultipleValueNode<TransactionSearchRequest, string> MerchantAccountId
        {
            get
            {
                return new MultipleValueNode<TransactionSearchRequest, string>("merchant-account-id", this);
            }
        }
        public TextNode<TransactionSearchRequest> OrderId
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("order-id", this);
            }
        }
        public TextNode<TransactionSearchRequest> PayPalAuthorizationId
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("paypal-authorization-id", this);
            }
        }
        public TextNode<TransactionSearchRequest> PayPalPayerEmail
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("paypal-payer-email", this);
            }
        }
        public TextNode<TransactionSearchRequest> PayPalPaymentId
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("paypal-payment-id", this);
            }
        }
        public TextNode<TransactionSearchRequest> PaymentMethodToken
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("payment-method-token", this);
            }
        }
        public TextNode<TransactionSearchRequest> ProcessorAuthorizationCode
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("processor-authorization-code", this);
            }
        }
        public DateRangeNode<TransactionSearchRequest> ProcessorDeclinedAt
        {
            get
            {
                return new DateRangeNode<TransactionSearchRequest>("processor-declined-at", this);
            }
        }
        public KeyValueNode<TransactionSearchRequest> Refund
        {
            get
            {
                return new KeyValueNode<TransactionSearchRequest>("refund", this);
            }
        }
        public DateRangeNode<TransactionSearchRequest> SettledAt
        {
            get
            {
                return new DateRangeNode<TransactionSearchRequest>("settled-at", this);
            }
        }
        public TextNode<TransactionSearchRequest> SettlementBatchId
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("settlement-batch-id", this);
            }
        }
        public TextNode<TransactionSearchRequest> ShippingCompany
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("shipping-company", this);
            }
        }
        public TextNode<TransactionSearchRequest> ShippingCountryName
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("shipping-country-name", this);
            }
        }
        public TextNode<TransactionSearchRequest> ShippingExtendedAddress
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("shipping-extended-address", this);
            }
        }
        public TextNode<TransactionSearchRequest> ShippingFirstName
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("shipping-first-name", this);
            }
        }
        public TextNode<TransactionSearchRequest> ShippingLastName
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("shipping-last-name", this);
            }
        }
        public TextNode<TransactionSearchRequest> ShippingLocality
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("shipping-locality", this);
            }
        }
        public TextNode<TransactionSearchRequest> ShippingPostalCode
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("shipping-postal-code", this);
            }
        }
        public TextNode<TransactionSearchRequest> ShippingRegion
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("shipping-region", this);
            }
        }
        public TextNode<TransactionSearchRequest> ShippingStreetAddress
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("shipping-street-address", this);
            }
        }
        public EnumMultipleValueNode<TransactionSearchRequest, TransactionStatus> Status
        {
            get
            {
                return new EnumMultipleValueNode<TransactionSearchRequest, TransactionStatus>("status", this);
            }
        }
        public DateRangeNode<TransactionSearchRequest> SubmittedForSettlementAt
        {
            get
            {
                return new DateRangeNode<TransactionSearchRequest>("submitted-for-settlement-at", this);
            }
        }
        public EnumMultipleValueNode<TransactionSearchRequest, TransactionSource> Source
        {
            get
            {
                return new EnumMultipleValueNode<TransactionSearchRequest, TransactionSource>("source", this);
            }
        }
        public EnumMultipleValueNode<TransactionSearchRequest, TransactionType> Type
        {
            get
            {
                return new EnumMultipleValueNode<TransactionSearchRequest, TransactionType>("type", this);
            }
        }
        public DateRangeNode<TransactionSearchRequest> VoidedAt
        {
            get
            {
                return new DateRangeNode<TransactionSearchRequest>("voided-at", this);
            }
        }

        public TransactionSearchRequest() : base()
        {
        }
    }
}

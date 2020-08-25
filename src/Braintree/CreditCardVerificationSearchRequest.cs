#pragma warning disable 1591

namespace Braintree
{
    public class CreditCardVerificationSearchRequest : SearchRequest
    {
        public TextNode<CreditCardVerificationSearchRequest> Id
        {
            get
            {
                return new TextNode<CreditCardVerificationSearchRequest>("id", this);
            }
        }
        public TextNode<CreditCardVerificationSearchRequest> CreditCardCardholderName
        {
            get
            {
                return new TextNode<CreditCardVerificationSearchRequest>("credit-card-cardholder-name", this);
            }
        }
        public EqualityNode<CreditCardVerificationSearchRequest> CreditCardExpirationDate
        {
            get
            {
                return new EqualityNode<CreditCardVerificationSearchRequest>("credit-card-expiration-date", this);
            }
        }
        public PartialMatchNode<CreditCardVerificationSearchRequest> CreditCardNumber
        {
            get
            {
                return new PartialMatchNode<CreditCardVerificationSearchRequest>("credit-card-number", this);
            }
        }
        public EnumMultipleValueNode<CreditCardVerificationSearchRequest, CreditCardCardType> CreditCardCardType
        {
            get
            {
                return new EnumMultipleValueNode<CreditCardVerificationSearchRequest, CreditCardCardType>("credit-card-card-type", this);
            }
        }
        public MultipleValueNode<CreditCardVerificationSearchRequest, string> Ids
        {
            get
            {
                return new MultipleValueNode<CreditCardVerificationSearchRequest, string>("ids", this);
            }
        }
        public DateRangeNode<CreditCardVerificationSearchRequest> CreatedAt
        {
            get
            {
                return new DateRangeNode<CreditCardVerificationSearchRequest>("created-at", this);
            }
        }
        public TextNode<CreditCardVerificationSearchRequest> PaymentMethodToken
        {
            get
            {
                return new TextNode<CreditCardVerificationSearchRequest>("payment-method-token", this);
            }
        }
        public TextNode<CreditCardVerificationSearchRequest> BillingAddressDetailsPostalCode
        {
            get
            {
                return new TextNode<CreditCardVerificationSearchRequest>("billing-address-details-postal-code", this);
            }
        }
        public TextNode<CreditCardVerificationSearchRequest> CustomerId
        {
            get
            {
                return new TextNode<CreditCardVerificationSearchRequest>("customer-id", this);
            }
        }
        public TextNode<CreditCardVerificationSearchRequest> CustomerEmail
        {
            get
            {
                return new TextNode<CreditCardVerificationSearchRequest>("customer-email", this);
            }
        }
        public EnumMultipleValueNode<CreditCardVerificationSearchRequest, VerificationStatus> Status
        {
            get
            {
                return new EnumMultipleValueNode<CreditCardVerificationSearchRequest, VerificationStatus>("status", this);
            }
        }
    }
}

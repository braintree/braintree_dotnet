#pragma warning disable 1591

namespace Braintree
{
    public class CreditCardVerificationSearchRequest : SearchRequest
    {
        public TextNode<CreditCardVerificationSearchRequest> Id => new TextNode<CreditCardVerificationSearchRequest>("id", this);

        public TextNode<CreditCardVerificationSearchRequest> CreditCardCardholderName => new TextNode<CreditCardVerificationSearchRequest>("credit-card-cardholder-name", this);

        public EqualityNode<CreditCardVerificationSearchRequest> CreditCardExpirationDate => new EqualityNode<CreditCardVerificationSearchRequest>("credit-card-expiration-date", this);

        public PartialMatchNode<CreditCardVerificationSearchRequest> CreditCardNumber => new PartialMatchNode<CreditCardVerificationSearchRequest>("credit-card-number", this);

        public EnumMultipleValueNode<CreditCardVerificationSearchRequest, CreditCardCardType> CreditCardCardType => new EnumMultipleValueNode<CreditCardVerificationSearchRequest, CreditCardCardType>("credit-card-card-type", this);

        public MultipleValueNode<CreditCardVerificationSearchRequest, string> Ids => new MultipleValueNode<CreditCardVerificationSearchRequest, string>("ids", this);

        public DateRangeNode<CreditCardVerificationSearchRequest> CreatedAt => new DateRangeNode<CreditCardVerificationSearchRequest>("created-at", this);

        public TextNode<CreditCardVerificationSearchRequest> PaymentMethodToken => new TextNode<CreditCardVerificationSearchRequest>("payment-method-token", this);

        public TextNode<CreditCardVerificationSearchRequest> BillingAddressDetailsPostalCode => new TextNode<CreditCardVerificationSearchRequest>("billing-address-details-postal-code", this);

        public TextNode<CreditCardVerificationSearchRequest> CustomerId => new TextNode<CreditCardVerificationSearchRequest>("customer-id", this);

        public TextNode<CreditCardVerificationSearchRequest> CustomerEmail => new TextNode<CreditCardVerificationSearchRequest>("customer-email", this);

        public EnumMultipleValueNode<CreditCardVerificationSearchRequest, VerificationStatus> Status => new EnumMultipleValueNode<CreditCardVerificationSearchRequest, VerificationStatus>("status", this);
    }
}

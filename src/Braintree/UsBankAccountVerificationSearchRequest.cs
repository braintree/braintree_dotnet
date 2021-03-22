#pragma warning disable 1591

namespace Braintree
{
    public class UsBankAccountVerificationSearchRequest : SearchRequest
    {
        public TextNode<UsBankAccountVerificationSearchRequest> Id => new TextNode<UsBankAccountVerificationSearchRequest>("id", this);

        public TextNode<UsBankAccountVerificationSearchRequest> AccountHolderName => new TextNode<UsBankAccountVerificationSearchRequest>("account-holder-name", this);

        public TextNode<UsBankAccountVerificationSearchRequest> RoutingNumber => new TextNode<UsBankAccountVerificationSearchRequest>("routing-number", this);

        public EndsWithNode<UsBankAccountVerificationSearchRequest> AccountNumber => new EndsWithNode<UsBankAccountVerificationSearchRequest>("account-number", this);

        public EqualityNode<UsBankAccountVerificationSearchRequest> AccountType => new EqualityNode<UsBankAccountVerificationSearchRequest>("account-type", this);

        public MultipleValueNode<UsBankAccountVerificationSearchRequest, string> Ids => new MultipleValueNode<UsBankAccountVerificationSearchRequest, string>("ids", this);

        public DateRangeNode<UsBankAccountVerificationSearchRequest> CreatedAt => new DateRangeNode<UsBankAccountVerificationSearchRequest>("created-at", this);

        public TextNode<UsBankAccountVerificationSearchRequest> PaymentMethodToken => new TextNode<UsBankAccountVerificationSearchRequest>("payment-method-token", this);

        public TextNode<UsBankAccountVerificationSearchRequest> CustomerId => new TextNode<UsBankAccountVerificationSearchRequest>("customer-id", this);

        public TextNode<UsBankAccountVerificationSearchRequest> CustomerEmail => new TextNode<UsBankAccountVerificationSearchRequest>("customer-email", this);

        public EnumMultipleValueNode<UsBankAccountVerificationSearchRequest, UsBankAccountVerificationStatus> Status => new EnumMultipleValueNode<UsBankAccountVerificationSearchRequest, UsBankAccountVerificationStatus>("status", this);

        public EnumMultipleValueNode<UsBankAccountVerificationSearchRequest, UsBankAccountVerificationMethod> VerificationMethod => new EnumMultipleValueNode<UsBankAccountVerificationSearchRequest, UsBankAccountVerificationMethod>("verification-method", this);
    }
}


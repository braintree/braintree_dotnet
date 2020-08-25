#pragma warning disable 1591

namespace Braintree
{
    public class UsBankAccountVerificationSearchRequest : SearchRequest
    {
        public TextNode<UsBankAccountVerificationSearchRequest> Id
        {
            get
            {
                return new TextNode<UsBankAccountVerificationSearchRequest>("id", this);
            }
        }

        public TextNode<UsBankAccountVerificationSearchRequest> AccountHolderName
        {
            get
            {
                return new TextNode<UsBankAccountVerificationSearchRequest>("account-holder-name", this);
            }
        }

        public TextNode<UsBankAccountVerificationSearchRequest> RoutingNumber
        {
            get
            {
                return new TextNode<UsBankAccountVerificationSearchRequest>("routing-number", this);
            }
        }

        public EndsWithNode<UsBankAccountVerificationSearchRequest> AccountNumber
        {
            get
            {
                return new EndsWithNode<UsBankAccountVerificationSearchRequest>("account-number", this);
            }
        }

        public EqualityNode<UsBankAccountVerificationSearchRequest> AccountType
        {
            get
            {
                return new EqualityNode<UsBankAccountVerificationSearchRequest>("account-type", this);
            }
        }

        public MultipleValueNode<UsBankAccountVerificationSearchRequest, string> Ids
        {
            get
            {
                return new MultipleValueNode<UsBankAccountVerificationSearchRequest, string>("ids", this);
            }
        }

        public DateRangeNode<UsBankAccountVerificationSearchRequest> CreatedAt
        {
            get
            {
                return new DateRangeNode<UsBankAccountVerificationSearchRequest>("created-at", this);
            }
        }

        public TextNode<UsBankAccountVerificationSearchRequest> PaymentMethodToken
        {
            get
            {
                return new TextNode<UsBankAccountVerificationSearchRequest>("payment-method-token", this);
            }
        }

        public TextNode<UsBankAccountVerificationSearchRequest> CustomerId
        {
            get
            {
                return new TextNode<UsBankAccountVerificationSearchRequest>("customer-id", this);
            }
        }

        public TextNode<UsBankAccountVerificationSearchRequest> CustomerEmail
        {
            get
            {
                return new TextNode<UsBankAccountVerificationSearchRequest>("customer-email", this);
            }
        }

        public EnumMultipleValueNode<UsBankAccountVerificationSearchRequest, UsBankAccountVerificationStatus> Status
        {
            get
            {
                return new EnumMultipleValueNode<UsBankAccountVerificationSearchRequest, UsBankAccountVerificationStatus>("status", this);
            }
        }

        public EnumMultipleValueNode<UsBankAccountVerificationSearchRequest, UsBankAccountVerificationMethod> VerificationMethod
        {
            get
            {
                return new EnumMultipleValueNode<UsBankAccountVerificationSearchRequest, UsBankAccountVerificationMethod>("verification-method", this);
            }
        }
    }
}


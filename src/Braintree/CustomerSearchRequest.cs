#pragma warning disable 1591

namespace Braintree
{
    public class CustomerSearchRequest : SearchRequest
    {
        public TextNode<CustomerSearchRequest> Id
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("id", this);
            }
        }
        public TextNode<CustomerSearchRequest> CardholderName
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("cardholder-name", this);
            }
        }
        public DateRangeNode<CustomerSearchRequest> CreatedAt
        {
            get
            {
                return new DateRangeNode<CustomerSearchRequest>("created-at", this);
            }
        }
        public PartialMatchNode<CustomerSearchRequest> CreditCardNumber
        {
            get
            {
                return new PartialMatchNode<CustomerSearchRequest>("credit-card-number", this);
            }
        }
        public MultipleValueNode<CustomerSearchRequest, string> Ids
        {
            get
            {
                return new MultipleValueNode<CustomerSearchRequest, string>("ids", this);
            }
        }
        public TextNode<CustomerSearchRequest> Website
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("website", this);
            }
        }
        public TextNode<CustomerSearchRequest> Email
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("email", this);
            }
        }
        public TextNode<CustomerSearchRequest> Company
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("company", this);
            }
        }
        public TextNode<CustomerSearchRequest> LastName
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("last-name", this);
            }
        }
        public TextNode<CustomerSearchRequest> FirstName
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("first-name", this);
            }
        }
        public TextNode<CustomerSearchRequest> Fax
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("fax", this);
            }
        }
        public TextNode<CustomerSearchRequest> Phone
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("Phone", this);
            }
        }
        public TextNode<CustomerSearchRequest> AddressFirstName
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("address-first-name", this);
            }
        }
        public TextNode<CustomerSearchRequest> AddressLastName
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("address-last-name", this);
            }
        }
        public TextNode<CustomerSearchRequest> AddressLocality
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("address-locality", this);
            }
        }
        public TextNode<CustomerSearchRequest> AddressPostalCode
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("address-postal-code", this);
            }
        }
        public TextNode<CustomerSearchRequest> AddressRegion
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("address-region", this);
            }
        }
        public TextNode<CustomerSearchRequest> AddressStreetAddress
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("address-street-address", this);
            }
        }
        public TextNode<CustomerSearchRequest> AddressExtendedAddress
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("address-extended-address", this);
            }
        }
        public TextNode<CustomerSearchRequest> AddressCountryName
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("address-country-name", this);
            }
        }
        public TextNode<CustomerSearchRequest> PaymentMethodToken
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("payment-method-token", this);
            }
        }
        public IsNode<CustomerSearchRequest> PaymentMethodTokenWithDuplicates
        {
            get
            {
                return new IsNode<CustomerSearchRequest>("payment-method-token-with-duplicates", this);
            }
        }
        public TextNode<CustomerSearchRequest> PayPalAccountEmail
        {
            get
            {
                return new TextNode<CustomerSearchRequest>("paypal-account-email", this);
            }
        }
        public EqualityNode<CustomerSearchRequest> CreditCardExpirationDate
        {
            get
            {
                return new EqualityNode<CustomerSearchRequest>("credit-card-expiration-date", this);
            }
        }
    }
}

#pragma warning disable 1591

namespace Braintree
{
    public class CustomerSearchRequest : SearchRequest
    {
        public TextNode<CustomerSearchRequest> Id => new TextNode<CustomerSearchRequest>("id", this);

        public TextNode<CustomerSearchRequest> CardholderName => new TextNode<CustomerSearchRequest>("cardholder-name", this);

        public DateRangeNode<CustomerSearchRequest> CreatedAt => new DateRangeNode<CustomerSearchRequest>("created-at", this);

        public PartialMatchNode<CustomerSearchRequest> CreditCardNumber => new PartialMatchNode<CustomerSearchRequest>("credit-card-number", this);

        public MultipleValueNode<CustomerSearchRequest, string> Ids => new MultipleValueNode<CustomerSearchRequest, string>("ids", this);

        public TextNode<CustomerSearchRequest> Website => new TextNode<CustomerSearchRequest>("website", this);

        public TextNode<CustomerSearchRequest> Email => new TextNode<CustomerSearchRequest>("email", this);

        public TextNode<CustomerSearchRequest> Company => new TextNode<CustomerSearchRequest>("company", this);

        public TextNode<CustomerSearchRequest> LastName => new TextNode<CustomerSearchRequest>("last-name", this);

        public TextNode<CustomerSearchRequest> FirstName => new TextNode<CustomerSearchRequest>("first-name", this);

        public TextNode<CustomerSearchRequest> Fax => new TextNode<CustomerSearchRequest>("fax", this);

        public TextNode<CustomerSearchRequest> Phone => new TextNode<CustomerSearchRequest>("Phone", this);

        public TextNode<CustomerSearchRequest> AddressFirstName => new TextNode<CustomerSearchRequest>("address-first-name", this);

        public TextNode<CustomerSearchRequest> AddressLastName => new TextNode<CustomerSearchRequest>("address-last-name", this);

        public TextNode<CustomerSearchRequest> AddressLocality => new TextNode<CustomerSearchRequest>("address-locality", this);

        public TextNode<CustomerSearchRequest> AddressPostalCode => new TextNode<CustomerSearchRequest>("address-postal-code", this);

        public TextNode<CustomerSearchRequest> AddressRegion => new TextNode<CustomerSearchRequest>("address-region", this);

        public TextNode<CustomerSearchRequest> AddressStreetAddress => new TextNode<CustomerSearchRequest>("address-street-address", this);

        public TextNode<CustomerSearchRequest> AddressExtendedAddress => new TextNode<CustomerSearchRequest>("address-extended-address", this);

        public TextNode<CustomerSearchRequest> AddressCountryName => new TextNode<CustomerSearchRequest>("address-country-name", this);

        public TextNode<CustomerSearchRequest> PaymentMethodToken => new TextNode<CustomerSearchRequest>("payment-method-token", this);

        public IsNode<CustomerSearchRequest> PaymentMethodTokenWithDuplicates => new IsNode<CustomerSearchRequest>("payment-method-token-with-duplicates", this);

        public TextNode<CustomerSearchRequest> PayPalAccountEmail => new TextNode<CustomerSearchRequest>("paypal-account-email", this);

        public EqualityNode<CustomerSearchRequest> CreditCardExpirationDate => new EqualityNode<CustomerSearchRequest>("credit-card-expiration-date", this);
    }
}

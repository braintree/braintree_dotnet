#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class TransactionSearchRequest : SearchRequest
    {
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
        public MultipleValueNode<TransactionSearchRequest> CreatedUsing
        {
            get
            {
                return new MultipleValueNode<TransactionSearchRequest>("created-using", this, TransactionCreatedUsing.ALL);
            }
        }
        public TextNode<TransactionSearchRequest> CreditCardCardholderName
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("credit-card-cardholder-name", this);
            }
        }
        public EqualityNode<TransactionSearchRequest> CreditCardExpirationDate
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("credit-card-expiration-date", this);
            }
        }
        public PartialMatchNode<TransactionSearchRequest> CreditCardNumber
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("credit-card-number", this);
            }
        }
        public MultipleValueNode<TransactionSearchRequest> CreditCardCardType
        {
            get
            {
                return new MultipleValueNode<TransactionSearchRequest>("credit-card-card-type", this, Braintree.CreditCardCardType.ALL);
            }
        }
        public MultipleValueNode<TransactionSearchRequest> CreditCardCustomerLocation
        {
            get
            {
                return new MultipleValueNode<TransactionSearchRequest>("credit-card-customer-location", this, Braintree.CreditCardCustomerLocation.ALL);
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
        public TextNode<TransactionSearchRequest> Id
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("id", this);
            }
        }
        public MultipleValueNode<TransactionSearchRequest> MerchantAccountId
        {
            get
            {
                return new MultipleValueNode<TransactionSearchRequest>("merchant-account-id", this);
            }
        }
        public TextNode<TransactionSearchRequest> OrderId
        {
            get
            {
                return new TextNode<TransactionSearchRequest>("order-id", this);
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
        public MultipleValueNode<TransactionSearchRequest> Status
        {
            get
            {
                return new MultipleValueNode<TransactionSearchRequest>("status", this, TransactionStatus.ALL);
            }
        }
        public MultipleValueNode<TransactionSearchRequest> Source
        {
            get
            {
                return new MultipleValueNode<TransactionSearchRequest>("source", this, TransactionSource.ALL);
            }
        }
        public MultipleValueNode<TransactionSearchRequest> Type
        {
            get
            {
                return new MultipleValueNode<TransactionSearchRequest>("type", this, TransactionType.ALL);
            }
        }
        public KeyValueNode<TransactionSearchRequest> Refund
        {
            get
            {
                return new KeyValueNode<TransactionSearchRequest>("refund", this);
            }
        }
        public TransactionSearchRequest() : base()
        {
        }
    }
}

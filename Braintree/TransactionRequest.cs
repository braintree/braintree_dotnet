#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class TransactionCreditCardRequest : BaseCreditCardRequest {}

    /// <summary>
    /// A class for building requests to manipulate <see cref="Transaction"/> records in the vault.
    /// </summary>
    /// <example>
    /// A transaction request can be constructed as follows:
    /// <code>
    /// var request = new TransactionRequest
    /// {
    ///     Amount = SandboxValues.TransactionAmount.AUTHORIZE,
    ///     CreditCard = new CreditCardRequest
    ///     {
    ///         Number = SandboxValues.CreditCardNumber.VISA,
    ///         ExpirationDate = "05/2009",
    ///     }
    /// };
    /// </code>
    /// </example>
    public class TransactionRequest : Request
    {
        public TransactionCreditCardRequest CreditCard { get; set; }
        public Decimal Amount { get; set; }
        public String Channel { get; set; }
        public String OrderId { get; set; }
        public Boolean? Recurring { get; set; }
        public String MerchantAccountId { get; set; }
        public String PurchaseOrderNumber { get; set; }
        public CustomerRequest Customer { get; set; }
        public DescriptorRequest Descriptor { get; set; }
        public AddressRequest BillingAddress { get; set; }
        public AddressRequest ShippingAddress { get; set; }
        public Decimal TaxAmount { get; set; }
        public Boolean? TaxExempt { get; set; }
        public TransactionType Type { get; set; }
        public Dictionary<String, String> CustomFields { get; set; }
        public TransactionOptionsRequest Options { get; set; }
        public String PaymentMethodToken { get; set; }
        public String CustomerId { get; set; }
        public String ShippingAddressId { get; set; }

        public TransactionRequest()
        {
            CustomFields = new Dictionary<String, String>();
        }

        public override String Kind()
        {
            return TransparentRedirectGateway.CREATE_TRANSACTION;
        }

        public override String ToXml()
        {
            return ToXml("transaction");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString()
        {
            return ToQueryString("transaction");
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            RequestBuilder builder = new RequestBuilder(root);

            if (Amount != 0) builder.AddElement("amount", Amount);
            builder.AddElement("customer-id", CustomerId);
            builder.AddElement("order-id", OrderId);
            builder.AddElement("channel", Channel);
            if (Recurring.HasValue) builder.AddElement("recurring", Recurring);
            builder.AddElement("payment-method-token", PaymentMethodToken);
            builder.AddElement("purchase-order-number", PurchaseOrderNumber);
            builder.AddElement("shipping-address-id", ShippingAddressId);
            if (TaxAmount != 0) builder.AddElement("tax-amount", TaxAmount);
            if (TaxExempt.HasValue) {
                builder.AddElement("tax-exempt", TaxExempt);
            }
            builder.AddElement("merchant-account-id", MerchantAccountId);

            if (Type != null) builder.AddElement("type", Type.ToString().ToLower());

            if (CustomFields.Count != 0) builder.AddElement("custom-fields", CustomFields);

            builder.AddElement("credit-card", CreditCard);
            builder.AddElement("customer", Customer);
            builder.AddElement("descriptor", Descriptor);
            builder.AddElement("billing", BillingAddress);
            builder.AddElement("shipping", ShippingAddress);
            builder.AddElement("options", Options);

            return builder;
        }
    }
}

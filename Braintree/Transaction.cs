using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    public enum TransactionStatus
    {
        AUTHORIZED, AUTHORIZING, FAILED, GATEWAY_REJECTED, PROCESSOR_DECLINED, SETTLED, SETTLEMENT_FAILED, SUBMITTED_FOR_SETTLEMENT, UNKNOWN, UNRECOGNIZED, VOIDED
    }

    public enum TransactionType
    {
        CREDIT, SALE, UNRECOGNIZED
    }

    public class Transaction
    {
        public String Id { get; protected set; }
        public Decimal? Amount { get; protected set; }
        public Address BillingAddressDetails { get; protected set; }
        public DateTime? CreatedAt { get; protected set; }
        public CreditCard CreditCardDetails { get; protected set; }
        public Customer CustomerDetails { get; protected set; }
        public String OrderId { get; protected set; }
        public String ProcessorResponseCode { get; protected set; }
        public String ProcessorResponseText { get; protected set; }
        public Address ShippingAddressDetails { get; protected set; }
        public TransactionStatus Status { get; protected set; }
        public TransactionType Type { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public Dictionary<String, String> CustomFields { get; protected set; }

        internal Transaction(NodeWrapper node)
        {
            if (node == null) return;

            Id = node.GetString("id");
            Amount = node.GetDecimal("amount");
            OrderId = node.GetString("order-id");
            Status = (TransactionStatus)EnumUtil.Find(typeof(TransactionStatus), node.GetString("status"), "unrecognized");
            Type = (TransactionType)EnumUtil.Find(typeof(TransactionType), node.GetString("type"), "unrecognized");
            ProcessorResponseCode = node.GetString("processor-response-code");
            ProcessorResponseText = node.GetString("processor-response-text");
            CustomFields = node.GetDictionary("custom-fields");
            CreditCardDetails = new CreditCard(node.GetNode("credit-card"));
            CustomerDetails = new Customer(node.GetNode("customer"));

            BillingAddressDetails = new Address(node.GetNode("billing"));
            ShippingAddressDetails = new Address(node.GetNode("shipping"));

            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");
        }

        public CreditCard GetVaultCreditCard()
        {
            if (CreditCardDetails.Token == null) return null;

            return new CreditCardGateway().Find(CreditCardDetails.Token);
        }

        public Customer GetVaultCustomer()
        {
            if (CustomerDetails.Id == null) return null;

            return new CustomerGateway().Find(CustomerDetails.Id);
        }

        public Address GetVaultBillingAddress()
        {
            if (BillingAddressDetails.Id == null) return null;

            return new AddressGateway().Find(CustomerDetails.Id, BillingAddressDetails.Id);
        }

        public Address GetVaultShippingAddress()
        {
            if (ShippingAddressDetails.Id == null) return null;

            return new AddressGateway().Find(CustomerDetails.Id, ShippingAddressDetails.Id);
        }
    }
}

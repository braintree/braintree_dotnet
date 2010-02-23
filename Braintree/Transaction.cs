using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    public enum TransactionStatus
    {
        AUTHORIZED, PROCESSOR_DECLINED, SETTLED, SUBMITTED_FOR_SETTLEMENT, VOIDED
    }

    public enum TransactionType
    {
        CREDIT, SALE
    }

    public class Transaction
    {
        public String ID { get; protected set; }
        public Decimal Amount { get; protected set; }
        public Address BillingAddressDetails { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public CreditCard CreditCardDetails { get; protected set; }
        public Customer CustomerDetails { get; protected set; }
        public String OrderId { get; protected set; }
        public String ProcessorResponseCode { get; protected set; }
        public String ProcessorResponseText { get; protected set; }
        public Address ShippingAddressDetails { get; protected set; }
        public TransactionStatus Status { get; protected set; }
        public TransactionType Type { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public Dictionary<String, String> CustomFields { get; protected set; }

        internal Transaction(NodeWrapper node)
        {
            if (node == null) return;

            ID = node.GetString("id");
            Amount = Decimal.Parse(node.GetString("amount"));
            OrderId = node.GetString("order-id");
            Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), node.GetString("status"), true);
            Type = (TransactionType)Enum.Parse(typeof(TransactionType), node.GetString("type"), true);
            ProcessorResponseCode = node.GetString("processor-response-code");
            ProcessorResponseText = node.GetString("processor-response-text");
            CustomFields = node.GetDictionary("custom-fields");
            CreditCardDetails = new CreditCard(node.GetNode("credit-card"));
            CustomerDetails = new Customer(node.GetNode("customer"));

            BillingAddressDetails = new Address(node.GetNode("billing"));
            ShippingAddressDetails = new Address(node.GetNode("shipping"));

            var createdAt = node.GetDateTime("created-at");
            if (createdAt != null) CreatedAt = (DateTime)createdAt;

            var updatedAt = node.GetDateTime("updated-at");
            if (updatedAt != null) UpdatedAt = (DateTime)updatedAt;
        }

        public CreditCard GetVaultCreditCard()
        {
            if (CreditCardDetails.Token == null) return null;

            return new CreditCardGateway().Find(CreditCardDetails.Token).Target;
        }

        public Customer GetVaultCustomer()
        {
            if (CustomerDetails.ID == null) return null;

            return new CustomerGateway().Find(CustomerDetails.ID).Target;
        }

        public Address GetVaultBillingAddress()
        {
            if (BillingAddressDetails.ID == null) return null;

            return new AddressGateway().Find(CustomerDetails.ID, BillingAddressDetails.ID).Target;
        }

        public Address GetVaultShippingAddress()
        {
            if (ShippingAddressDetails.ID == null) return null;

            return new AddressGateway().Find(CustomerDetails.ID, ShippingAddressDetails.ID).Target;
        }
    }
}

#pragma warning disable 1591

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

    /// <summary>
    /// A transaction returned by the Braintree Gateway
    /// </summary>
    /// <example>
    /// Transactions can be retrieved via the gateway using the associated transaction id:
    /// <code>
    ///     Transaction transaction = gateway.Transaction.Find("transactionId");
    /// </code>
    /// </example>
    public class Transaction
    {
        public String Id { get; protected set; }
        public Decimal? Amount { get; protected set; }
        public Address BillingAddress { get; protected set; }
        public DateTime? CreatedAt { get; protected set; }
        public CreditCard CreditCard { get; protected set; }
        public Customer Customer { get; protected set; }
        public String OrderId { get; protected set; }
        public String ProcessorAuthorizationCode { get; protected set; }
        public String ProcessorResponseCode { get; protected set; }
        public String ProcessorResponseText { get; protected set; }
        public Address ShippingAddress { get; protected set; }
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
            ProcessorAuthorizationCode = node.GetString("processor-authorization-code");
            ProcessorResponseCode = node.GetString("processor-response-code");
            ProcessorResponseText = node.GetString("processor-response-text");
            CustomFields = node.GetDictionary("custom-fields");
            CreditCard = new CreditCard(node.GetNode("credit-card"));
            Customer = new Customer(node.GetNode("customer"));

            BillingAddress = new Address(node.GetNode("billing"));
            ShippingAddress = new Address(node.GetNode("shipping"));

            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");
        }

        /// <summary>
        /// Returns the current <see cref="CreditCard"/> associated with this transaction if one exists
        /// </summary>
        /// <returns>
        /// The current <see cref="CreditCard"/> associated with this transaction if one exists
        /// </returns>
        /// <remarks>
        /// When retrieving a transaction from the gateway, the credit card used in the transaction is returned in the response.
        /// If the credit card record has been updated in the vault since the transaction occurred, this method can be used to 
        /// retrieve the updated credit card information.  This is typically useful in situations where a transaction fails, for
        /// example when a credit card expires, and a new transaction needs to be submitted once the new credit card information
        /// has been submitted.
        /// </remarks>
        /// <example>
        /// The vault <see cref="CreditCard"/> can be retrieved from the transaction directly:
        /// <code>
        ///     Transaction transaction = gateway.Transaction.Find("transactionId");
        ///     CreditCard creditCard = transaction.GetVaultCreditCard();
        /// </code>
        /// </example>
        /// <example>
        /// Failed transactions can be resubmitted with updated <see cref="CreditCard"/> information:
        /// <code>
        ///     Transaction failedTransaction = gateway.Transaction.Find("transactionId");
        ///     CreditCard updatedCreditCard = transaction.GetVaultCreditCard();
        ///
        ///     TransactionRequest request = new TransactionRequest
        ///     {
        ///         Amount = failedTransaction.Amount,
        ///         PaymentMethodToken = updatedCreditCard.Token
        ///     };
        ///     
        ///     Result&lt;Transaction&gt; result = gateway.Transaction.Sale(request);
        /// </code>
        /// </example>
        public virtual CreditCard GetVaultCreditCard()
        {
            if (CreditCard.Token == null) return null;

            return new CreditCardGateway().Find(CreditCard.Token);
        }

        /// <summary>
        /// Returns the current <see cref="Customer"/> associated with this transaction if one exists
        /// </summary>
        /// <returns>
        /// The current <see cref="Customer"/> associated with this transaction if one exists
        /// </returns>
        /// <remarks>
        /// When retrieving a transaction from the gateway, the customer associated with the transaction is returned in the response.
        /// If the customer record has been updated in the vault since the transaction occurred, this method can be used to 
        /// retrieve the updated customer information.
        /// </remarks>
        /// <example>
        /// The vault <see cref="Customer"/> can be retrieved from the transaction directly:
        /// <code>
        ///     Transaction transaction = gateway.Transaction.Find("transactionId");
        ///     Customer customer = transaction.GetVaultCustomer();
        /// </code>
        /// </example>
        public virtual Customer GetVaultCustomer()
        {
            if (Customer.Id == null) return null;

            return new CustomerGateway().Find(Customer.Id);
        }

        /// <summary>
        /// Returns the current billing <see cref="Address"/> associated with this transaction if one exists
        /// </summary>
        /// <returns>
        /// The current billing <see cref="Address"/> associated with this transaction if one exists
        /// </returns>
        /// <remarks>
        /// When retrieving a transaction from the gateway, the billing address associated with the transaction is returned in the response.
        /// If the billing address has been updated in the vault since the transaction occurred, this method can be used to 
        /// retrieve the updated billing address.
        /// </remarks>
        /// <example>
        /// The vault billing <see cref="Address"/> can be retrieved from the transaction directly:
        /// <code>
        ///     Transaction transaction = gateway.Transaction.Find("transactionId");
        ///     Address billingAddress = transaction.GetVaultBillingAddress();
        /// </code>
        /// </example>
        public virtual Address GetVaultBillingAddress()
        {
            if (BillingAddress.Id == null) return null;

            return new AddressGateway().Find(Customer.Id, BillingAddress.Id);
        }

        /// <summary>
        /// Returns the current shipping <see cref="Address"/> associated with this transaction if one exists
        /// </summary>
        /// <returns>
        /// The current shipping <see cref="Address"/> associated with this transaction if one exists
        /// </returns>
        /// <remarks>
        /// When retrieving a transaction from the gateway, the shipping address associated with the transaction is returned in the response.
        /// If the shipping address has been updated in the vault since the transaction occurred, this method can be used to 
        /// retrieve the updated shipping address.
        /// </remarks>
        /// <example>
        /// The vault shipping <see cref="Address"/> can be retrieved from the transaction directly:
        /// <code>
        ///     Transaction transaction = gateway.Transaction.Find("transactionId");
        ///     Address shippingAddress = transaction.GetVaultShippingAddress();
        /// </code>
        /// </example>
        public virtual Address GetVaultShippingAddress()
        {
            if (ShippingAddress.Id == null) return null;

            return new AddressGateway().Find(Customer.Id, ShippingAddress.Id);
        }
    }
}

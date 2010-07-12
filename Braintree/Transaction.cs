#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    public class TransactionGatewayRejectionReason : Enumeration
    {
        public static readonly TransactionGatewayRejectionReason AVS = new TransactionGatewayRejectionReason("avs");
        public static readonly TransactionGatewayRejectionReason AVS_AND_CVV = new TransactionGatewayRejectionReason("avs_and_cvv");
        public static readonly TransactionGatewayRejectionReason CVV = new TransactionGatewayRejectionReason("cvv");
        public static readonly TransactionGatewayRejectionReason DUPLICATE = new TransactionGatewayRejectionReason("duplicate");

        public static readonly TransactionGatewayRejectionReason[] ALL = {
            AVS, AVS_AND_CVV, CVV, DUPLICATE
        };

        protected TransactionGatewayRejectionReason(String name) : base(name) {}
    }

    public class TransactionStatus : Enumeration
    {
        public static readonly TransactionStatus AUTHORIZED = new TransactionStatus("authorized");
        public static readonly TransactionStatus AUTHORIZING = new TransactionStatus("authorizing");
        public static readonly TransactionStatus FAILED = new TransactionStatus("failed");
        public static readonly TransactionStatus GATEWAY_REJECTED = new TransactionStatus("gateway_rejected");
        public static readonly TransactionStatus PROCESSOR_DECLINED = new TransactionStatus("processor_declined");
        public static readonly TransactionStatus SETTLED = new TransactionStatus("settled");
        public static readonly TransactionStatus SETTLEMENT_FAILED = new TransactionStatus("settlement_failed");
        public static readonly TransactionStatus SUBMITTED_FOR_SETTLEMENT = new TransactionStatus("submitted_for_settlement");
        public static readonly TransactionStatus UNRECOGNIZED = new TransactionStatus("unrecognized");
        public static readonly TransactionStatus VOIDED = new TransactionStatus("voided");

        public static readonly TransactionStatus[] ALL = {
            AUTHORIZED, AUTHORIZING, FAILED, GATEWAY_REJECTED, PROCESSOR_DECLINED, SETTLED,
            SETTLEMENT_FAILED, SUBMITTED_FOR_SETTLEMENT, VOIDED
        };

        protected TransactionStatus(String name) : base(name) {}
    }

    public class TransactionSource : Enumeration
    {
        public static readonly TransactionSource API = new TransactionSource("api");
        public static readonly TransactionSource CONTROL_PANEL = new TransactionSource("control_panel");
        public static readonly TransactionSource RECURRING = new TransactionSource("recurring");
        public static readonly TransactionSource UNRECOGNIZED = new TransactionSource("unrecognized");

        public static readonly TransactionSource[] ALL = { API, CONTROL_PANEL, RECURRING };

        protected TransactionSource(String name) : base(name) {}
    }

    public class TransactionType : Enumeration
    {
        public static readonly TransactionType CREDIT = new TransactionType("credit");
        public static readonly TransactionType SALE = new TransactionType("sale");
        public static readonly TransactionType UNRECOGNIZED = new TransactionType("unrecognized");

        public static readonly TransactionType[] ALL = { CREDIT, SALE };

        protected TransactionType(String name) : base(name) {}
    }

    public abstract class TransactionCreatedUsing
    {
        public const String FULL_INFORMATION = "full_information";
        public const String TOKEN = "token";

        public static readonly String[] ALL = {FULL_INFORMATION, TOKEN};
    }

    /// <summary>
    /// A transaction returned by the Braintree Gateway
    /// </summary>
    /// <example>
    /// Transactions can be retrieved via the gateway using the associated transaction id:
    /// <code>
    ///     Transaction transaction = gateway.Transaction.Find("transactionId");
    /// </code>
    /// For more information about Transactions, see <a href="http://www.braintreepaymentsolutions.com/gateway/transaction-api" target="_blank">http://www.braintreepaymentsolutions.com/gateway/transaction-api</a>
    /// </example>
    public class Transaction
    {
        public String Id { get; protected set; }
        public Decimal? Amount { get; protected set; }
        public String AvsErrorResponseCode { get; protected set; }
        public String AvsPostalCodeResponseCode { get; protected set; }
        public String AvsStreetAddressResponseCode { get; protected set; }
        public Address BillingAddress { get; protected set; }
        public DateTime? CreatedAt { get; protected set; }
        public CreditCard CreditCard { get; protected set; }
        public String CurrencyIsoCode { get; protected set; }
        public Customer Customer { get; protected set; }
        public String CvvResponseCode { get; protected set; }
        public TransactionGatewayRejectionReason GatewayRejectionReason { get; protected set; }
        public String MerchantAccountId { get; protected set; }
        public String OrderId { get; protected set; }
        public String ProcessorAuthorizationCode { get; protected set; }
        public String ProcessorResponseCode { get; protected set; }
        public String ProcessorResponseText { get; protected set; }
        public String RefundedTransactionId { get; protected set; }
        public String RefundId { get; protected set; }
        public String SettlementBatchId { get; protected set; }
        public Address ShippingAddress { get; protected set; }
        public TransactionStatus Status { get; protected set; }
        public StatusEvent[] StatusHistory { get; protected set; }
        public String SubscriptionId { get; protected set; }
        public TransactionType Type { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public Dictionary<String, String> CustomFields { get; protected set; }

        private BraintreeService Service;

        internal Transaction(NodeWrapper node, BraintreeService service)
        {
            Service = service;

            if (node == null) return;

            Id = node.GetString("id");
            Amount = node.GetDecimal("amount");
            AvsErrorResponseCode = node.GetString("avs-error-response-code");
            AvsPostalCodeResponseCode = node.GetString("avs-postal-code-response-code");
            AvsStreetAddressResponseCode = node.GetString("avs-street-address-response-code");
            GatewayRejectionReason = (TransactionGatewayRejectionReason)CollectionUtil.Find(
                TransactionGatewayRejectionReason.ALL,
                node.GetString("gateway-rejection-reason"),
                null
            );
            OrderId = node.GetString("order-id");
            Status = (TransactionStatus)CollectionUtil.Find(TransactionStatus.ALL, node.GetString("status"), TransactionStatus.UNRECOGNIZED);

            List<NodeWrapper> statusNodes = node.GetList("status-history/status-event");
            StatusHistory = new StatusEvent[statusNodes.Count];
            for (int i = 0; i < statusNodes.Count; i++)
            {
                StatusHistory[i] = new StatusEvent(statusNodes[i]);
            }

            Type = (TransactionType)CollectionUtil.Find(TransactionType.ALL, node.GetString("type"), TransactionType.UNRECOGNIZED);
            MerchantAccountId = node.GetString("merchant-account-id");
            ProcessorAuthorizationCode = node.GetString("processor-authorization-code");
            ProcessorResponseCode = node.GetString("processor-response-code");
            ProcessorResponseText = node.GetString("processor-response-text");
            RefundedTransactionId = node.GetString("refunded-transaction-id");
            RefundId = node.GetString("refund-id");
            SettlementBatchId = node.GetString("settlement-batch-id");
            SubscriptionId = node.GetString("subscription-id");
            CustomFields = node.GetDictionary("custom-fields");
            CreditCard = new CreditCard(node.GetNode("credit-card"), service);
            Customer = new Customer(node.GetNode("customer"), service);
            CurrencyIsoCode = node.GetString("currency-iso-code");
            CvvResponseCode = node.GetString("cvv-response-code");

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

            return new CreditCardGateway(Service).Find(CreditCard.Token);
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

            return new CustomerGateway(Service).Find(Customer.Id);
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

            return new AddressGateway(Service).Find(Customer.Id, BillingAddress.Id);
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

            return new AddressGateway(Service).Find(Customer.Id, ShippingAddress.Id);
        }
    }
}

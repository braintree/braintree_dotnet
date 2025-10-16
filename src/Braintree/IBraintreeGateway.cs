#pragma warning disable 1591

namespace Braintree
{
    /// <summary>
    /// This is the primary interface to the Braintree Gateway.
    /// </summary>
    /// <remarks>
    /// This class interacts with:
    /// <ul>
    /// <li><see cref="AddressGateway">Addresses</see></li>
    /// <li><see cref="CreditCardGateway">CreditCards</see></li>
    /// <li><see cref="CustomerGateway">Customers</see></li>
    /// <li><see cref="SubscriptionGateway">Subscriptions</see></li>
    /// <li><see cref="TransactionGateway">Transactions</see></li>
    /// </ul>
    /// </remarks>
    /// <example>
    /// Quick Start Example:
    /// <code>
    /// using System;
    /// using Braintree;
    ///
    /// namespace BraintreeExample
    /// {
    ///     class Program
    ///     {
    ///         static void Main(string[] args)
    ///         {
    ///             var gateway = new BraintreeGateway
    ///             {
    ///                 Environment = Braintree.Environment.SANDBOX,
    ///                 MerchantId = "the_merchant_id",
    ///                 PublicKey = "a_public_key",
    ///                 PrivateKey = "a_private_key"
    ///             };
    ///
    ///             var request = new TransactionRequest
    ///             {
    ///                 Amount = 100.00M,
    ///                 PaymentMethodNonce = nonceFromTheClient
    ///             };
    ///
    ///             Transaction transaction = gateway.Transaction.Sale(request).Target;
    ///
    ///             Console.WriteLine(string.Format("Transaction ID: {0}", transaction.Id));
    ///             Console.WriteLine(string.Format("Status: {0}", transaction.Status));
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public interface IBraintreeGateway
    {
        BraintreeService Service { get; }
        Configuration Configuration { get; }
        Environment Environment { get; set; }
        IAddOnGateway AddOn { get; }
        IAddressGateway Address { get; }
        IBankAccountInstantVerificationGateway BankAccountInstantVerification { get; }
        IClientTokenGateway ClientToken { get; }
        ICreditCardGateway CreditCard { get; }
        ICreditCardVerificationGateway CreditCardVerification { get; }
        ICustomerGateway Customer { get; }
        IDiscountGateway Discount { get; }
        IDisputeGateway Dispute { get; }
        IDocumentUploadGateway DocumentUpload { get; }
        IExchangeRateQuoteGateway ExchangeRateQuote {get;}
        IMerchantAccountGateway MerchantAccount { get; }
        IMerchantGateway Merchant { get; }
        IOAuthGateway OAuth { get; }
        IPayPalAccountGateway PayPalAccount { get; }
        IPaymentMethodGateway PaymentMethod { get; }
        IPaymentMethodNonceGateway PaymentMethodNonce { get; }
        IPlanGateway Plan { get; }
        ISepaDirectDebitAccountGateway SepaDirectDebitAccount { get; }
        ISettlementBatchSummaryGateway SettlementBatchSummary { get; }
        ISubscriptionGateway Subscription { get; }
        ITestTransactionGateway TestTransaction { get; }
        ITransactionGateway Transaction { get; }
        ITransactionLineItemGateway TransactionLineItem { get; }
        IWebhookNotificationGateway WebhookNotification { get; }
        IWebhookTestingGateway WebhookTesting { get; }
        string AccessToken { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string MerchantId { get; set; }
        string PrivateKey { get; set; }
        string PublicKey { get; set; }
    }
}

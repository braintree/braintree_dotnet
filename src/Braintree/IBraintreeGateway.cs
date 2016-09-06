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
        string AccessToken { get; set; }
        IAddOnGateway AddOn { get; }
        IAddressGateway Address { get; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        IClientTokenGateway ClientToken { get; }
        Configuration Configuration { get; }
        ICreditCardGateway CreditCard { get; }
        ICreditCardVerificationGateway CreditCardVerification { get; }
        ICustomerGateway Customer { get; }
        IDiscountGateway Discount { get; }
        Environment Environment { get; set; }
        IMerchantGateway Merchant { get; }
        IMerchantAccountGateway MerchantAccount { get; }
        string MerchantId { get; set; }
        IOAuthGateway OAuth { get; }
        IPaymentMethodGateway PaymentMethod { get; }
        IPaymentMethodNonceGateway PaymentMethodNonce { get; }
        IPayPalAccountGateway PayPalAccount { get; }
        IPlanGateway Plan { get; }
        string PrivateKey { get; set; }
        string PublicKey { get; set; }
        ISettlementBatchSummaryGateway SettlementBatchSummary { get; }
        ISubscriptionGateway Subscription { get; }
        ITestTransactionGateway TestTransaction { get; }
        ITransactionGateway Transaction { get; }
        ITransparentRedirectGateway TransparentRedirect { get; }
        IWebhookNotificationGateway WebhookNotification { get; }
        IWebhookTestingGateway WebhookTesting { get; }

        string TrData(Request trData, string redirectURL);
    }
}

#pragma warning disable 1591

namespace Braintree
{
    /// <summary>
    /// This is the primary interface to the Braintree Gateway.
    /// </summary>
    /// <remarks>
    /// This class interact with:
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
    ///                 CreditCard = new CreditCardRequest
    ///                 {
    ///                     Number = "5105105105105100",
    ///                     ExpirationDate = "05/12"
    ///                 }
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
    public class BraintreeGateway : IBraintreeGateway
    {
        public virtual Environment Environment
        {
            get => Configuration.Environment;
            set => Configuration.Environment = value;
        }

        public virtual string MerchantId
        {
            get => Configuration.MerchantId;
            set => Configuration.MerchantId = value;
        }

        public virtual string PublicKey
        {
            get => Configuration.PublicKey;
            set => Configuration.PublicKey = value;
        }

        public virtual string PrivateKey
        {
            get => Configuration.PrivateKey;
            set => Configuration.PrivateKey = value;
        }

        public virtual string AccessToken
        {
            get => Configuration.AccessToken;
            set => Configuration.AccessToken = value;
        }

        public virtual string ClientId
        {
            get => Configuration.ClientId;
            set => Configuration.ClientId = value;
        }

        public virtual string ClientSecret
        {
            get => Configuration.ClientSecret;
            set => Configuration.ClientSecret = value;
        }

        public Configuration Configuration { get; set; }

        public GraphQLClient GraphQLClient { get; set; }

        public BraintreeService Service { get; }

        public BraintreeGateway()
        {
            Configuration = new Configuration();
            GraphQLClient = new GraphQLClient(Configuration);
            Service = new BraintreeService(Configuration);
        }

        public BraintreeGateway(Environment environment, string merchantId, string publicKey, string privateKey)
        {
            Configuration = new Configuration(environment, merchantId, publicKey, privateKey);
            GraphQLClient = new GraphQLClient(Configuration);
            Service = new BraintreeService(Configuration);
        }

        public BraintreeGateway(string environment, string merchantId, string publicKey, string privateKey)
        {
            Configuration = new Configuration(Environment.ParseEnvironment(environment), merchantId, publicKey, privateKey);
            GraphQLClient = new GraphQLClient(Configuration);
            Service = new BraintreeService(Configuration);
        }

        public BraintreeGateway(string accessToken)
        {
            Configuration = new Configuration(accessToken);
            GraphQLClient = new GraphQLClient(Configuration);
            Service = new BraintreeService(Configuration);
        }

        public BraintreeGateway(string clientId, string clientSecret)
        {
            Configuration = new Configuration(clientId, clientSecret);
            GraphQLClient = new GraphQLClient(Configuration);
            Service = new BraintreeService(Configuration);
        }

        public BraintreeGateway(Configuration configuration)
        {
            Configuration = configuration;
            GraphQLClient = new GraphQLClient(Configuration);
            Service = new BraintreeService(Configuration);
        }

        public virtual IClientTokenGateway ClientToken => new ClientTokenGateway(this);

        public virtual ICustomerGateway Customer => new CustomerGateway(this);

        public virtual IAddressGateway Address => new AddressGateway(this);

        public virtual IAddOnGateway AddOn => new AddOnGateway(this);

        public virtual ICreditCardGateway CreditCard => new CreditCardGateway(this);

        public virtual ICreditCardVerificationGateway CreditCardVerification => new CreditCardVerificationGateway(this);

        public virtual IUsBankAccountVerificationGateway UsBankAccountVerification => new UsBankAccountVerificationGateway(this);


        public virtual IDiscountGateway Discount => new DiscountGateway(this);

        public virtual IDisputeGateway Dispute => new DisputeGateway(this);

        public virtual IDocumentUploadGateway DocumentUpload => new DocumentUploadGateway(this);

        public virtual IMerchantAccountGateway MerchantAccount => new MerchantAccountGateway(this);

        public virtual IOAuthGateway OAuth => new OAuthGateway(this);

        public virtual IMerchantGateway Merchant => new MerchantGateway(this);

        public virtual IPaymentMethodGateway PaymentMethod => new PaymentMethodGateway(this);

        public virtual IPayPalAccountGateway PayPalAccount => new PayPalAccountGateway(this);

        public virtual IPlanGateway Plan => new PlanGateway(this);

        public virtual ISettlementBatchSummaryGateway SettlementBatchSummary => new SettlementBatchSummaryGateway(this);

        public virtual ISubscriptionGateway Subscription => new SubscriptionGateway(this);

        public virtual IThreeDSecureGateway ThreeDSecure => new ThreeDSecureGateway(this);

        public virtual ITestTransactionGateway TestTransaction => new TestTransactionGateway(this);

        public virtual ITransactionGateway Transaction => new TransactionGateway(this);

        public virtual ITransactionLineItemGateway TransactionLineItem => new TransactionLineItemGateway(this);

        public virtual IWebhookNotificationGateway WebhookNotification => new WebhookNotificationGateway(this);

        public virtual IWebhookTestingGateway WebhookTesting => new WebhookTestingGateway(this);

        public virtual IPaymentMethodNonceGateway PaymentMethodNonce => new PaymentMethodNonceGateway(this);

        public virtual IExchangeRateQuoteGateway ExchangeRateQuote => new ExchangeRateQuoteGateway(this);
    }
}
